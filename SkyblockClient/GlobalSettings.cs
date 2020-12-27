using System.ComponentModel;

namespace SkyblockClient
{
    public class GlobalSettings
    {
        const bool IGNORE_OUTDATED_VERSION_DEFAULT = true;
        const bool APPEND_MISSING_OPTION_ICON_DEFAULT = true;
        const bool COPY_ALL_ENDPOINTS_TO_CLIPBOARD_ON_DEBUG_DEFAULT = false;
        const bool UPDATE_PROFILE_ON_DEBUG_DEFAULT = false;
        const bool COPY_ENDPOINT_TO_CLIPBOARD_ON_DEBUG_ON_ERROR_DEFAULT = true;
        const bool ENABLE_MOD_DEPENDENT_PACKS_ON_ENABLE_DEFAULT = true;

        [DefaultValue(APPEND_MISSING_OPTION_ICON_DEFAULT)]
        public bool appendMissingOptionIcon = APPEND_MISSING_OPTION_ICON_DEFAULT;

        [DefaultValue(IGNORE_OUTDATED_VERSION_DEFAULT)]
        public bool ignoreOutdatedVersion = IGNORE_OUTDATED_VERSION_DEFAULT;

        [DefaultValue(COPY_ALL_ENDPOINTS_TO_CLIPBOARD_ON_DEBUG_DEFAULT)]
        public bool copyAllEndpointsToClipboardOnDebug = COPY_ALL_ENDPOINTS_TO_CLIPBOARD_ON_DEBUG_DEFAULT;

        [DefaultValue(UPDATE_PROFILE_ON_DEBUG_DEFAULT)]
        public bool updateProfileOnDebug = UPDATE_PROFILE_ON_DEBUG_DEFAULT;

        [DefaultValue(COPY_ENDPOINT_TO_CLIPBOARD_ON_DEBUG_ON_ERROR_DEFAULT)]
        public bool copyEndpointToClipboardOnDebugOnError = COPY_ENDPOINT_TO_CLIPBOARD_ON_DEBUG_ON_ERROR_DEFAULT;

        [DefaultValue(ENABLE_MOD_DEPENDENT_PACKS_ON_ENABLE_DEFAULT)]
        public bool enableModDependentPacksOnEnable = ENABLE_MOD_DEPENDENT_PACKS_ON_ENABLE_DEFAULT;
    }
}
