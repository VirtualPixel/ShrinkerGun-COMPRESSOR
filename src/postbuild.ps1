param(
    [string]$Version,
    [string]$DllPath,
    [string]$RepoRoot
)

$RepoRoot = [System.IO.Path]::GetFullPath($RepoRoot)
$buildZipDir = Join-Path $RepoRoot "BuildZip\ShrinkerGun"
$enc = New-Object System.Text.UTF8Encoding $false

function Update-ManifestVersion([string]$path, [string]$version) {
    $content = [System.IO.File]::ReadAllText($path, $enc)
    $content = $content -replace '"version_number":\s*"[^"]*"', "`"version_number`": `"$version`""
    [System.IO.File]::WriteAllText($path, $content, $enc)
}

# Ensure build dir exists
New-Item -ItemType Directory -Path $buildZipDir -Force | Out-Null

# Update manifest version from csproj Version (single source of truth)
Update-ManifestVersion (Join-Path $RepoRoot "manifest.json") $Version

# Copy DLL and release assets
Copy-Item -LiteralPath $DllPath -Destination $buildZipDir -Force
Copy-Item -LiteralPath (Join-Path $RepoRoot "README.md") -Destination $buildZipDir -Force
Copy-Item -LiteralPath (Join-Path $RepoRoot "CHANGELOG.md") -Destination $buildZipDir -Force
Copy-Item -LiteralPath (Join-Path $RepoRoot "manifest.json") -Destination $buildZipDir -Force

# Repobundle: required for the gun's custom asset
$bundleSrc = Get-ChildItem -Path $RepoRoot -Filter "*.repobundle" -Recurse -File | Select-Object -First 1
if ($bundleSrc) {
    Copy-Item -LiteralPath $bundleSrc.FullName -Destination (Join-Path $buildZipDir $bundleSrc.Name) -Force
    Write-Host "Included repobundle: $($bundleSrc.Name)"
} else {
    Write-Host "WARNING: No .repobundle found - zip will be missing asset bundle"
}

# Icon: copy if exists, skip if not
$iconSrc = Join-Path $RepoRoot "icon.png"
if (Test-Path $iconSrc) {
    Copy-Item -LiteralPath $iconSrc -Destination (Join-Path $buildZipDir "icon.png") -Force
} else {
    Write-Host "SKIP: icon.png not found at $iconSrc - add it before uploading"
}

# Create zip (exclude any existing zip)
$zipPath = Join-Path $buildZipDir "ShrinkerGun.zip"
$files = Get-ChildItem -LiteralPath $buildZipDir -File | Where-Object { $_.Extension -ne ".zip" }
if ($files.Count -gt 0) {
    Compress-Archive -Path $files.FullName -DestinationPath $zipPath -Force
    Write-Host "Packaged v$Version -> $zipPath"
} else {
    Write-Host "WARNING: No files to package"
}

# Clear BepInEx log for a clean test run (silently skip if locked)
try { [System.IO.File]::WriteAllText("$env:APPDATA\com.kesomannen.gale\repo\profiles\Development\BepInEx\LogOutput.log", "", $enc) }
catch { }
