# Microsoft Developer Studio Generated NMAKE File, Format Version 4.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Console Application" 0x0103

!IF "$(CFG)" == ""
CFG=Neitkriv - Win32 Debug
!MESSAGE No configuration specified.  Defaulting to Neitkriv - Win32 Debug.
!ENDIF 

!IF "$(CFG)" != "Neitkriv - Win32 Release" && "$(CFG)" !=\
 "Neitkriv - Win32 Debug"
!MESSAGE Invalid configuration "$(CFG)" specified.
!MESSAGE You can specify a configuration when running NMAKE on this makefile
!MESSAGE by defining the macro CFG on the command line.  For example:
!MESSAGE 
!MESSAGE NMAKE /f "Neitkriv.mak" CFG="Neitkriv - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "Neitkriv - Win32 Release" (based on\
 "Win32 (x86) Console Application")
!MESSAGE "Neitkriv - Win32 Debug" (based on "Win32 (x86) Console Application")
!MESSAGE 
!ERROR An invalid configuration is specified.
!ENDIF 

!IF "$(OS)" == "Windows_NT"
NULL=
!ELSE 
NULL=nul
!ENDIF 
################################################################################
# Begin Project
# PROP Target_Last_Scanned "Neitkriv - Win32 Debug"
RSC=rc.exe
F90=fl32.exe

!IF  "$(CFG)" == "Neitkriv - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
OUTDIR=.
INTDIR=.

ALL : "$(OUTDIR)\Neitkriv.exe"

CLEAN : 
	-@erase ".\Neitkriv.exe"
	-@erase ".\rungekutt.obj"
	-@erase ".\F.obj"
	-@erase ".\Neitkriv.obj"

# ADD BASE F90 /Ox /c /nologo
# ADD F90 /Ox /c /nologo
F90_PROJ=/Ox /c /nologo 
# ADD BASE RSC /l 0x419 /d "NDEBUG"
# ADD RSC /l 0x419 /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
BSC32_FLAGS=/nologo /o"$(OUTDIR)/Neitkriv.bsc" 
BSC32_SBRS=
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib /nologo /subsystem:console /machine:I386
# ADD LINK32 kernel32.lib /nologo /subsystem:console /machine:I386
LINK32_FLAGS=kernel32.lib /nologo /subsystem:console /incremental:no\
 /pdb:"$(OUTDIR)/Neitkriv.pdb" /machine:I386 /out:"$(OUTDIR)/Neitkriv.exe" 
LINK32_OBJS= \
	"$(INTDIR)/rungekutt.obj" \
	"$(INTDIR)/F.obj" \
	"$(INTDIR)/Neitkriv.obj"

"$(OUTDIR)\Neitkriv.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "Neitkriv - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 1
OUTDIR=.
INTDIR=.

ALL : "$(OUTDIR)\Neitkriv.exe"

CLEAN : 
	-@erase ".\Neitkriv.exe"
	-@erase ".\rungekutt.obj"
	-@erase ".\F.obj"
	-@erase ".\Neitkriv.obj"
	-@erase ".\Neitkriv.ilk"
	-@erase ".\Neitkriv.pdb"

# ADD BASE F90 /Zi /c /nologo
# ADD F90 /Zi /c /nologo
F90_PROJ=/Zi /c /nologo /Fd"Neitkriv.pdb" 
# ADD BASE RSC /l 0x419 /d "_DEBUG"
# ADD RSC /l 0x419 /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
BSC32_FLAGS=/nologo /o"$(OUTDIR)/Neitkriv.bsc" 
BSC32_SBRS=
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib /nologo /subsystem:console /debug /machine:I386
# ADD LINK32 kernel32.lib /nologo /subsystem:console /debug /machine:I386
LINK32_FLAGS=kernel32.lib /nologo /subsystem:console /incremental:yes\
 /pdb:"$(OUTDIR)/Neitkriv.pdb" /debug /machine:I386\
 /out:"$(OUTDIR)/Neitkriv.exe" 
LINK32_OBJS= \
	"$(INTDIR)/rungekutt.obj" \
	"$(INTDIR)/F.obj" \
	"$(INTDIR)/Neitkriv.obj"

"$(OUTDIR)\Neitkriv.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ENDIF 

.for.obj:
   $(F90) $(F90_PROJ) $<  

.f.obj:
   $(F90) $(F90_PROJ) $<  

.f90.obj:
   $(F90) $(F90_PROJ) $<  

################################################################################
# Begin Target

# Name "Neitkriv - Win32 Release"
# Name "Neitkriv - Win32 Debug"

!IF  "$(CFG)" == "Neitkriv - Win32 Release"

!ELSEIF  "$(CFG)" == "Neitkriv - Win32 Debug"

!ENDIF 

################################################################################
# Begin Source File

SOURCE=.\Neitkriv.for

"$(INTDIR)\Neitkriv.obj" : $(SOURCE) "$(INTDIR)"


# End Source File
################################################################################
# Begin Source File

SOURCE=.\rungekutt.for

"$(INTDIR)\rungekutt.obj" : $(SOURCE) "$(INTDIR)"


# End Source File
################################################################################
# Begin Source File

SOURCE=.\F.for

"$(INTDIR)\F.obj" : $(SOURCE) "$(INTDIR)"


# End Source File
# End Target
# End Project
################################################################################
