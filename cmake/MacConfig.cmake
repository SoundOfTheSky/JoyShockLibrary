if (APPLE)
    set (MACOS ON)
    find_package (PkgConfig QUIET REQUIRED)
    pkg_search_module (HIDAPI REQUIRED IMPORTED_TARGET hidapi)
	add_library (
		jsl_platform_dependencies INTERFACE
	)
	target_link_libraries (
		jsl_platform_dependencies INTERFACE
        PkgConfig::HIDAPI
    )
    add_library (JSL_Platform::Dependencies ALIAS jsl_platform_dependencies)
endif ()