SUBDIRS = . NUnit

FILES =  \
	AssemblyInfo.cs \
	Commands.cs \
	MonoMakefile.cs \
	MonoMakefileFormat.cs \
	MonoSolutionItemHandler.cs

DEPS =  \
	$(top_builddir)/build/bin/MonoDevelop.Core.dll \
	$(top_builddir)/build/bin/MonoDevelop.Ide.dll

REFS =  \
	$(GTK_SHARP_LIBS) \
	$(MONO_ADDINS_LIBS) \
	-r:System \
	-r:System.Data \
	-r:System.Xml

RES = MonoDeveloperExtensions.addin.xml

ADDIN_BUILD = $(top_builddir)/build/AddIns/MonoDeveloperExtensions

ASSEMBLY = $(ADDIN_BUILD)/MonoDeveloperExtensions.dll

if ENABLE_MONOEXTENSIONS
all: $(ASSEMBLY) $(ASSEMBLY).mdb
else
all:
endif

$(ASSEMBLY): $(build_sources) $(srcdir)/$(GLADEFILE) $(build_resources) $(DEPS)
	mkdir -p $(ADDIN_BUILD)
	$(CSC) $(CSC_FLAGS) -target:library -out:$@ $(build_resources:%=/resource:%) $(build_sources) $(REFS) $(build_deps)

$(ASSEMBLY).mdb: $(ASSEMBLY)

CLEANFILES = $(ASSEMBLY) $(ASSEMBLY).mdb

EXTRA_DIST = $(FILES) $(RES)

include $(top_srcdir)/Makefile.include

if ENABLE_MONOEXTENSIONS
MonoDeveloperExtensionsdir = $(MD_ADDIN_DIR)/MonoDeveloperExtensions
MonoDeveloperExtensions_DATA = $(ASSEMBLY) $(ASSEMBLY).mdb
endif
