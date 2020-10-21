using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient
{
	class ModOptionDefaults
	{
		public ModOptionStructure optifine = new ModOptionStructure(
			"optifine", 
			"Optifine L6",
			true); 
		public ModOptionStructure patcher = new ModOptionStructure(
			"patcher", 
			"Patcher 1.4",
			true);
		public ModOptionStructure sba = new ModOptionStructure(
			"sba", 
			"SkyblockAddons",
			true);
		public ModOptionStructure neu = new ModOptionStructure(
			"neu",
			"Not Enough Updates",
			true);
		public ModOptionStructure scrollabletooltips = new ModOptionStructure(
			"scrollabletooltips",
			"Scrollable Tooltips",
			true);
		public ModOptionStructure sidebarmod = new ModOptionStructure(
			"sidebarmod",
			"Sidebar mod",
			true);
		public ModOptionStructure rpo = new ModOptionStructure(
			"rpo",
			"Recource Pack Organizer",
			true);
		public ModOptionStructure hytilities = new ModOptionStructure(
			"hytilities",
			"Hytilities",
			true);
		public ModOptionStructure togglesneak = new ModOptionStructure(
			"togglesneak",
			"Pown's Toggle Sneak",
			false);
		public ModOptionStructure ctjs = new ModOptionStructure(
			"ctjs",
			"Chat Triggers",
			false);
		public ModOptionStructure discordrp = new ModOptionStructure(
			"discordrp",
			"Discord Rich Pressence",
			false);
		public ModOptionStructure oldanim = new ModOptionStructure(
			"oldanim",
			"Old Animations",
			false);
	}
}
