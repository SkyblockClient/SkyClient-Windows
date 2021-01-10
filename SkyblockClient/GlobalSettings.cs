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
        const bool INSTALL_FORGE_LIBRARIES_DEFAULT = true;
        const bool INSTALL_PROFILE_TO_LAUNCHER = true;
        const bool CHECK_JAVA_VERSION_DEFAULT = false;
        const bool CHECK_SIMILARITIES_ON_UPDATE = true;
        const bool IGNORE_MISSING_PERSITENCE = true;

        const int SIMILARITIES_THRESHOLD_DEFAULT = 6;

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

        [DefaultValue(INSTALL_FORGE_LIBRARIES_DEFAULT)]
        public bool installForgeLibaries = INSTALL_FORGE_LIBRARIES_DEFAULT;

        [DefaultValue(INSTALL_PROFILE_TO_LAUNCHER)]
        public bool installProfileToLauncher = INSTALL_PROFILE_TO_LAUNCHER;

        [DefaultValue(CHECK_JAVA_VERSION_DEFAULT)]
        public bool checkJavaVersion = CHECK_JAVA_VERSION_DEFAULT;

        /// <summary>
        /// when the users presses update, this will check if there are files in the same 
        /// directory that could be a known mod
        /// example: 
        /// Compared to preview_OptiFine_1.8.9_HD_U_M5_pre2.jar
        /// preview OptiFine 1.8.9 HD U M5 pre2.jar has 6 differences
        /// OptiFine_1.8.9_HD_U_M5_pre2.jar has 8 differences
        /// </summary>
        [DefaultValue(CHECK_SIMILARITIES_ON_UPDATE)]
        public bool checkSimilaritiesOnUpdate = CHECK_SIMILARITIES_ON_UPDATE;

        /// <summary>
        /// the threshold that decides whether a mod is a known mod or not
        /// after testing, 6 has revealed itself to be a good value
        /// if a threshold value is the threshold or lower, it will be accepted
        /// </summary>
        [DefaultValue(SIMILARITIES_THRESHOLD_DEFAULT)]
        public int similaritiesThreshold = SIMILARITIES_THRESHOLD_DEFAULT;

        [DefaultValue(IGNORE_MISSING_PERSITENCE)]
        public bool ignoreMissingPersistence = IGNORE_MISSING_PERSITENCE;
    }
}
