/*
 * Copyright (C) 2007 Eitan Pogrebizsky <openpandora@gmail.com>, 
 * and individual contributors.
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

;--------------------------------
;Include Modern UI

  !include "MUI.nsh"
  
  !include WordFunc.nsh
  !insertmacro VersionCompare
  !include LogicLib.nsh

;--------------------------------
;General

  ; TODO: Install for current or all users SetShellVarContext

  ;Version
  !define OPEN_PANDORA_VERSION "0.7.0.6"

  ;Name and file
  Name "OpenPandora"
  OutFile "OpenPandora_${OPEN_PANDORA_VERSION}.exe"
  BrandingText " "
  
  VIProductVersion "${OPEN_PANDORA_VERSION}.0"
  VIAddVersionKey "ProductName" "OpenPandora"
  VIAddVersionKey "Comments" " "
  VIAddVersionKey "CompanyName" "Eitan Pogrebizsky"
  VIAddVersionKey "LegalTrademarks" " "
  VIAddVersionKey "LegalCopyright" "© 2009 Eitan Pogrebizsky"
  VIAddVersionKey "FileDescription" "OpenPandora Setup"
  VIAddVersionKey "FileVersion" "${OPEN_PANDORA_VERSION}"
  
  SetCompressor /SOLID LZMA

  ;Default installation folder
  InstallDir "$PROGRAMFILES\OpenPandora"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\OpenPandora" ""

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING
  !define MUI_COMPONENTSPAGE_NODESC
  !define MUI_WELCOMEPAGE_TITLE "Welcome to OpenPandora ${OPEN_PANDORA_VERSION} Setup Wizard"
  
  !define MUI_FINISHPAGE_RUN
  !define MUI_FINISHPAGE_RUN_TEXT "Run OpenPandora"
  !define MUI_FINISHPAGE_RUN_FUNCTION "LaunchOpenPandora"
  !define MUI_ICON "..\Bitmap\Pandora.ico"
  !define MUI_UNICON "uninstall.ico"
  
  !define DOTNET_URL "http://www.microsoft.com/downloads/details.aspx?FamilyID=262d25e3-f589-4842-8157-034d1e7cf3a3"

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_LICENSE "..\License.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  !insertmacro MUI_PAGE_FINISH
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Functions

Function .onInit
  
  ;Set install types
  InstTypeSetText 0 "Full"
  InstTypeSetText 1 "Minimal"
  
  ;
  ;.NET version
  
  Call GetDotNETVersion
  Pop $0
  ${If} $0 == "not found"
    MessageBox MB_OK|MB_ICONSTOP ".NET Framework v1.1 is required.$\n$\nClosing this message will open a link with the missing component download location."
    ExecShell "open" ${DOTNET_URL}
    Abort
  ${EndIf}
 
  StrCpy $0 $0 "" 1 # skip "v"
 
  ${VersionCompare} $0 "1.1" $1
  ${If} $1 == 2
    MessageBox MB_OK|MB_ICONSTOP ".NET Framework v1.1 is required.$\n$\nClosing this message will open a link with the missing component download location."
    ExecShell "open" ${DOTNET_URL}
    Abort
  ${EndIf}  
FunctionEnd
 
Function GetDotNETVersion
  Push $0
  Push $1
 
  System::Call "mscoree::GetCORVersion(w .r0, i ${NSIS_MAX_STRLEN}, *i) i .r1"
  StrCmp $1 "error" 0 +2
    StrCpy $0 "not found"
 
  Pop $1
  Exch $0
FunctionEnd

Function LaunchOpenPandora
  ExecShell "" "$INSTDIR\OpenPandora.exe"
FunctionEnd

;--------------------------------
;Installer Sections

Section "!Main Files" OpenPandora
SectionIn RO 
  SetOutPath "$INSTDIR"
  
  ;ADD YOUR OWN FILES HERE...
  File ..\bin\Debug\AxInterop.SHDocVw.dll
  File ..\bin\Debug\Interop.SHDocVw.dll
  File ..\bin\Debug\OpenPandora.exe
  File Microsoft.mshtml.dll
  
  ;Store installation folder
  WriteRegStr HKLM "Software\OpenPandora" "InstallLocation" $INSTDIR
  
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenPandora" "DisplayName" "OpenPandora ${OPEN_PANDORA_VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenPandora" "InstallLocation" $INSTDIR
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenPandora" "UninstallString" "$INSTDIR\uninstall.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenPandora" "DisplayVersion" "${OPEN_PANDORA_VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenPandora" "HelpLink" "http://openpandora.googlepages.com"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenPandora" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenPandora" "NoRepair" 1
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

Section "Start Menu Group" StartMenu
SectionIn 1
  CreateDirectory "$SMPROGRAMS\OpenPandora"
  CreateShortCut "$SMPROGRAMS\OpenPandora\OpenPandora.lnk" "$INSTDIR\OpenPandora.exe" "" "$INSTDIR\OpenPandora.exe" 0
  CreateShortCut "$SMPROGRAMS\OpenPandora\Uninstall.lnk" "$INSTDIR\Uninstall.exe" "" "$INSTDIR\Uninstall.exe" 0
SectionEnd

Section "Desktop Shortcut" DesktopShortcut
SectionIn 1
  CreateShortCut "$DESKTOP\OpenPandora.lnk" "$INSTDIR\OpenPandora.exe" "" "$INSTDIR\OpenPandora.exe" 0
SectionEnd

Section /o "Quick Launch Shortcut" QuickLaunchShortcut
SectionIn 1
  CreateShortCut "$QUICKLAUNCH\OpenPandora.lnk" "$INSTDIR\OpenPandora.exe" "" "$INSTDIR\OpenPandora.exe" 0
SectionEnd

Section /o "Launch at startup" LaunchAtStartup
SectionIn 1
  CreateShortCut "$SMSTARTUP\OpenPandora.lnk" "$INSTDIR\OpenPandora.exe" "" "$INSTDIR\OpenPandora.exe" 0
SectionEnd

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  ;ADD YOUR OWN FILES HERE...

  Delete "$INSTDIR\Uninstall.exe"
  Delete "$INSTDIR\AxInterop.SHDocVw.dll"
  Delete "$INSTDIR\Interop.SHDocVw.dll"
  Delete "$INSTDIR\Microsoft.mshtml.dll"
  Delete "$INSTDIR\OpenPandora.exe"
  
  Delete "$DESKTOP\OpenPandora.lnk"
  Delete "$QUICKLAUNCH\OpenPandora.lnk"
  
  Delete "$SMPROGRAMS\OpenPandora\OpenPandora.lnk"
  Delete "$SMPROGRAMS\OpenPandora\Uninstall.lnk"
  Delete "$SMSTARTUP\OpenPandora.lnk"
  RMDir "$SMPROGRAMS\OpenPandora"

  RMDir "$INSTDIR"

  DeleteRegKey /ifempty HKCU "Software\OpenPandora"
  DeleteRegKey /ifempty HKLM "Software\OpenPandora"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenPandora"

SectionEnd