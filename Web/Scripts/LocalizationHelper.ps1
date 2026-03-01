# Batch Localization Helper Script
# This script helps identify and localize remaining components

Write-Host "=== Blazor Localization Helper ===" -ForegroundColor Cyan
Write-Host ""

$componentsPath = "H:\Odai\SMA\Retail\Web\Components"

# Function to find hard-coded strings (simple pattern matching)
function Find-HardCodedStrings {
    param($filePath)
    
    $content = Get-Content $filePath -Raw
    
    # Find potential hard-coded strings in common patterns
    $patterns = @(
        '<h\d[^>]*>([A-Z][a-zA-Z\s]+)</h\d>',  # Headings
        '<button[^>]*>([A-Z][a-zA-Z\s]+)</button>',  # Buttons
        '<label[^>]*>([A-Z][a-zA-Z\s]+)</label>',  # Labels
        '<span[^>]*>([A-Z][a-zA-Z\s]{3,})</span>',  # Span text
        '<p[^>]*>([A-Z][a-zA-Z\s]{5,})</p>'  # Paragraphs
    )
    
    $found = @()
    foreach ($pattern in $patterns) {
        $matches = [regex]::Matches($content, $pattern)
        foreach ($match in $matches) {
            if ($match.Groups[1].Value -notmatch '@' -and 
                $match.Groups[1].Value -notmatch '\d{4}' -and
                $match.Groups[1].Value.Length -gt 2) {
                $found += $match.Groups[1].Value.Trim()
            }
        }
    }
    
    return $found | Select-Object -Unique
}

# Get all razor files
$razorFiles = Get-ChildItem -Path $componentsPath -Recurse -Filter "*.razor" | 
    Where-Object { $_.Name -notlike "*.*.*" -and $_.Name -ne "LocalizationExample.razor" }

Write-Host "Found $($razorFiles.Count) Razor components" -ForegroundColor Green
Write-Host ""

# Priority pages
$priorityPages = @(
    "AdminLayout.razor",
    "Dashboard.razor",
    "Users.razor",
    "UserForm.razor",
    "Roles.razor",
    "RoleForm.razor",
    "Profile.razor",
    "ChangePassword.razor",
    "Settings.razor"
)

Write-Host "=== HIGH PRIORITY COMPONENTS ===" -ForegroundColor Yellow
Write-Host ""

foreach ($priority in $priorityPages) {
    $file = $razorFiles | Where-Object { $_.Name -eq $priority } | Select-Object -First 1
    if ($file) {
        Write-Host "?? $($file.Name)" -ForegroundColor Cyan
        Write-Host "   Path: $($file.FullName.Replace($componentsPath, 'Components'))" -ForegroundColor Gray
        
        # Check if it has @inject IJsonStringLocalizer
        $content = Get-Content $file.FullName -Raw
        $hasLocalizer = $content -match '@inject\s+IJsonStringLocalizer'
        
        if ($hasLocalizer) {
            Write-Host "   ? Has Localizer injected" -ForegroundColor Green
        } else {
            Write-Host "   ??  Needs Localizer (but it's globally injected via _Imports.razor)" -ForegroundColor Yellow
        }
        
        # Find potential hard-coded strings
        $strings = Find-HardCodedStrings -filePath $file.FullName
        if ($strings.Count -gt 0) {
            Write-Host "   ?? Found $($strings.Count) potential hard-coded strings:" -ForegroundColor Magenta
            $strings | Select-Object -First 5 | ForEach-Object {
                Write-Host "      - $_" -ForegroundColor Gray
            }
            if ($strings.Count -gt 5) {
                Write-Host "      ... and $($strings.Count - 5) more" -ForegroundColor DarkGray
            }
        }
        
        Write-Host ""
    }
}

Write-Host ""
Write-Host "=== SUMMARY ===" -ForegroundColor Cyan
Write-Host "Total components: $($razorFiles.Count)"
Write-Host "High priority components identified: $($priorityPages.Count)"
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Green
Write-Host "1. Localize AdminLayout.razor (main navigation)" -ForegroundColor White
Write-Host "2. Localize Dashboard pages" -ForegroundColor White
Write-Host "3. Localize form pages (UserForm, RoleForm, etc.)" -ForegroundColor White
Write-Host ""
Write-Host "Note: Localizer is globally available via _Imports.razor" -ForegroundColor Yellow
Write-Host "You can use @Localizer[""Key""] in any component!" -ForegroundColor Yellow
