using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KsWare.Windows.Internal {

	internal static class WinVer {

		// Windows 10, version 1803	=> 10.0.17134

		// 
		private static readonly Dictionary<string, Version> vmap10 = new Dictionary<string, Version>() {
			{"RTM", new Version(10, 0, 10240)},
			{"1507", new Version(10, 0, 10240)},
			{"1511", new Version(10, 0, 10586)},	// “November Update”						10. Nov. 2015
			{"1607", new Version(10, 0, 14393)},	// “Anniversary Update”						 2. Aug. 2016
			{"1703", new Version(10, 0, 15063)},	// "Spring Creators Update",  “Redstone 2”	11. Apr. 2017	
			{"1709", new Version(10, 0, 16299)},	// "Fall Creators Update",					17. Okt. 2017
			{"1803", new Version(10, 0, 17134)},
			{"1809", new Version(10, 0, 17763)},
			{"1903", new Version(10, 0, 18362)},
			{"1909", new Version(10, 0, 18363)},
			{"2004", new Version(10, 0, 19041)},
			{"20H2", new Version(10, 0, 19042)},
			{"21H1", new Version(10, 0, 19043)},
			{"21H2", new Version(10, 0, 19044)},
		};
		private static readonly Dictionary<string, Version> vmap11 = new Dictionary<string, Version>() {
			{"21H2", new Version(11,0,22000)},
		};

		
		public static bool IsWindows10v1709OrNewer => Environment.OSVersion.Version >= vmap10["1709"];
		public static bool IsWindows10v1803OrNewer => Environment.OSVersion.Version >= vmap10["1803"];

		public static bool IsEqualOrNewerAs(string version) {
			// "MM VVVV" 
			var match=Regex.Match(version, @"(?<major>\d+)[^\d]+(?<version>[0-9H]{4})",RegexOptions.Compiled);
			if (!match.Success) throw new ArgumentException("Unsupported version format");
			var major = int.Parse(match.Groups["major"].Value);
			var ver = match.Groups["version"].Value;
			switch (major) {
				case 10: return Environment.OSVersion.Version >= vmap10[ver];
				case 11: return Environment.OSVersion.Version >= vmap11[ver];
				default: throw new ArgumentException("Unsupported version format");
			}
		}
	}

}
