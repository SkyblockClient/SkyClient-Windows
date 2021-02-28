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
        const bool CHECK_SIMILARITIES_ON_UPDATE_ADVANCED = true;
        const bool SKYCLIENT_JSON_USE_CLOUD_IMAGE = false;
		const bool ENABLE_UPDATE_BUTTON_DEFAULT = true;

        const int SIMILARITIES_THRESHOLD_DEFAULT = 6;
        const int SIMILARITIES_THRESHOLD_ADVANCED_DEFAULT = 0;

        const string IMAGE_BASE64_DEFAULT = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAMAAAD04JH5AAAACGFjVEwAAAAEAAAAAHzNZtAAAAAzUExURf//ACOQY5HbaR68c////11dXXR0dIWFhc3fbNDq6T09PWG/5j+Y2KjQ2Sdyxtrq+k2b5npKrGcAAAABdFJOUwBA5thmAAAAGmZjVEwAAAAAAAAAgAAAAIAAAAAAAAAAAAAIAGQAAK+px9AAAAKZSURBVHja7ZvpbgIxDISBQrcFerz/0zYjZarp4F0OocYgz5898PEhWXESwmp1Rpuu1SgVQAqAl6Z7QNwUowCGAjA5tO6as/1o2nbN2cD/KogCSAHw1kSAyBkJ37u2IrdjISNeATwGAI0pFqFCeFJ9xr0mp28BPAaAFgyTR4ORJ8RgxIL0ePBjvALID8CEdGBTigqRTQjXpYZGv4uaUQEMBWASB4Cu+hZN9Hf4Rf8CGArAAUgHjY2In9Pm0ma2NhVAXgBNxORaOFpMDqBBdSLDGK4CyAegAw+LzZuIBtOEXMDqQiYqQC3yE4gCGA5AZx94IgB/1k0MHbAiAIUogDwAPvlgIUUTCW9IGlQ3tebswkIsgKEAvhDVhYgaewKfYCxBRfEKIDdAFMATfDYduw5NHlhjaAG6TQGMB9Aiigowak6HrqMIQOcAfGJaADkA9Mckbxo6yXBnbEyxALExOTW9NvkEdG5h8pu8AFIAvHbtm2iw69JnJCEUN6gBgPupC3G0APmecXgPuwLIB4AkgNBgfO8AKED+YIFBaBLBZjIhDu8RqwByAfAFr2qsACww3HMQggBAnyiGxqHNn5VxAQwHIAQKUAtPA3hgAqAIcaWv20UxCyA3AAPwfglAC3HfpXb6pXaicKe8AIYDQAxARzpPgTQ5RH9PHsUsgLwACqEAPpnAFQ2IC1QCeGNjMj/oUgC5AQhBZwb0SaoCQJE/Dzb5gTc/6FQAuQD8gJo2kQiAEEsHnVVnD0AXwFCAr6bogOrOhALTRhQBePF5zALIB6AfRgOIN6nvprmByAuRg4/GPWlKBTAcwJPqM5uHTjKRlBtUOiG5WQUwHEAhzk0cdZOqAJ4L4BqxOAGw9Ae3AnhOgGiy8a8QBXBPgB9G8drRaA0Z2QAAABpmY1RMAAAAAQAAABQAAAA8AAAANAAAADwACABkAAETQ/wcAAAAWmZkQVQAAAACeNq1kzEOACAIAx1cjMT/P9eSSCLqJOU2joVAKQOUk6BUOmBLRUAFLLkX2mTIbNg3u7ZEkExssS4UQZmZ1WcQPmXGf9rAbvCgNCELlrRwNRCUEzPhDNF8OvhOAAAAGmZjVEwAAAADAAAAFAAAAEgAAAA4AAAAOAAIAGQCAb6WRGwAAABrZmRBVAAAAAR42s2TQQ6AIAwEOXgxEv//XEvCmmK97Zg4F8L0AC1L60F78kN5BtuEkkewCEB+0f8elKIpx+W1klKFMXBCitdBm5JEQ1ADhMxBKwVTkpTHAmQOGCVzYJdDAamA3Z/NlGXjyQth3xPxgCBbFQAAABpmY1RMAAAABQAAABQAAABEAAAANAAAADwACABkAAHaykukAAAAc2ZkQVQAAAAGeNq1lDEKwDAMAz10KS39/3MrDxIu2SrllphzII5xUjcocAKtpmRwgN5wgYR8QMuaBCQPUPGmZOEdq3hTdsBmMJmQJCl5CTXZlDtYGhyWSphykpI9tEvWlDsKnoP7OfSn5FDNB+bKiT4AT76lBBmhtuxsJwAAABh0RVh0U29mdHdhcmUAZ2lmMmFwbmcuc2YubmV0lv8TyAAAAABJRU5ErkJggg==";
        const string JAVA_ARGS_DEFAULT = "-Xmx3G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=32M";

        public string version;

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

		[DefaultValue(ENABLE_UPDATE_BUTTON_DEFAULT)]
		public bool enableUpdateButton = ENABLE_UPDATE_BUTTON_DEFAULT;

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

        /// <summary>
        /// the threshold that decides whether a mod is a known mod or not
        /// after testing, 0 has revealed itself to be a good value
        /// if a threshold value is the threshold or lower, it will be accepted
        /// this threshold is used when checking in a more advanced alogrithm
        /// </summary>
        [DefaultValue(SIMILARITIES_THRESHOLD_ADVANCED_DEFAULT)]
        public int similaritiesThresholdAdvanced = SIMILARITIES_THRESHOLD_ADVANCED_DEFAULT;

        [DefaultValue(JAVA_ARGS_DEFAULT)]
        public string javaArgs = JAVA_ARGS_DEFAULT;

        [DefaultValue(IMAGE_BASE64_DEFAULT)]
        public string imageBase64 = IMAGE_BASE64_DEFAULT;

        [DefaultValue(SKYCLIENT_JSON_USE_CLOUD_IMAGE)]
        public bool SkyClientJsonUseCloudImage = SKYCLIENT_JSON_USE_CLOUD_IMAGE;
	}
}
