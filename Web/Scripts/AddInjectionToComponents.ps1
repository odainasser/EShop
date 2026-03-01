# PowerShell script to add injection to all Razor components
# This is for reference - NOT NEEDED since Localizer is already in _Imports.razor

param(
    [string]$ComponentsPath = "H:\Odai\SMA\Retail\Web\Components",
    [string]$InjectLine = "@inject IJsonStringLocalizer Localizer"
)

Write-Host "=== Add Injection to Razor Components ===" -ForegroundColor Cyan
Write-Host ""

# Get all .razor files (excluding generated files)
$razorFiles = Get-ChildItem -Path $ComponentsPath -Recurse -Filter "*.razor" | 
    Where-Object { 
        $_.Name -notlike "*.*.*" -and 
        $_.Name -ne "_Imports.razor" -and
        $_.DirectoryName -notlike "*\obj\*" -and
        $_.DirectoryName -notlike "*\bin\*"
    }

Write-Host "Found $($razorFiles.Count) Razor files" -ForegroundColor Green
Write-Host ""

$filesProcessed = 0
$filesSkipped = 0
$filesUpdated = 0

foreach ($file in $razorFiles) {
    $content = Get-Content $file.FullName -Raw
    
    # Check if injection already exists
    if ($content -match "@inject\s+IJsonStringLocalizer") {
        Write-Host "? SKIP: $($file.Name) - Already has Localizer" -ForegroundColor Gray
        $filesSkipped++
        continue
    }
    
    # Find where to insert (after @page, @layout, @using directives)
    $lines = Get-Content $file.FullName
    $insertIndex = 0
    
    # Find the last directive line (@page, @layout, @using, @inject, @inherits)
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match "^@(page|layout|using|inject|inherits|attribute)") {
            $insertIndex = $i + 1
        }
        elseif ($lines[$i] -notmatch "^\s*$" -and $lines[$i] -notmatch "^@\*") {
            # Stop at first non-directive, non-comment, non-blank line
            break
        }
    }
    
    # Insert the injection line
    $newLines = @()
    $newLines += $lines[0..($insertIndex - 1)]
    $newLines += $InjectLine
    $newLines += $lines[$insertIndex..($lines.Count - 1)]
    
    # Write back to file
    try {
        Set-Content -Path $file.FullName -Value $newLines -Force
        Write-Host "? UPDATED: $($file.Name)" -ForegroundColor Green
        $filesUpdated++
    }
    catch {
        Write-Host "? ERROR: $($file.Name) - $($_.Exception.Message)" -ForegroundColor Red
    }
    
    $filesProcessed++
}

Write-Host ""
Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "Total files found: $($razorFiles.Count)" -ForegroundColor White
Write-Host "Files updated: $filesUpdated" -ForegroundColor Green
Write-Host "Files skipped: $filesSkipped" -ForegroundColor Yellow
Write-Host "Files processed: $filesProcessed" -ForegroundColor White
Write-Host ""

if ($filesUpdated -gt 0) {
    Write-Host "? Complete! $filesUpdated files updated." -ForegroundColor Green
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Yellow
    Write-Host "1. Review changes with: git diff" -ForegroundColor White
    Write-Host "2. Build the project: dotnet build" -ForegroundColor White
    Write-Host "3. Test the application" -ForegroundColor White
}
else {
    Write-Host "No files needed updating." -ForegroundColor Yellow
}
