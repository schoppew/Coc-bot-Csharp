using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.BotEngine.ScreenReading;

namespace CoC.Bot.Data
{
	static public partial class ScreenData
	{
		#region OCR Data
		static public Dictionary<char, OCRChar> GetOCRData()
		{
			return new Dictionary<char, OCRChar>
		{		
			{ 'A', new OCRChar( 7, new OCRPoint(1, 8, 0xE7E7E7, 6), new OCRPoint(5, 7, 0xDBDCDB, 6), new OCRPoint(6, 8, 0xD5D6D5, 6)) },
			{ 'a', new OCRChar( 5, new OCRPoint(5, 3, 0xACAEAC, 6), new OCRPoint(1, 4, 0xC2C3C2, 6), new OCRPoint(4, 7, 0xDADBDA, 6)) },
			{ 'B', new OCRChar( 7, new OCRPoint(6, 1, 0xE2E3E2, 6), new OCRPoint(1, 1, 0xFBFBFB, 6), new OCRPoint(3, 4, 0xE8E9E8, 6)) },
			{ 'b', new OCRChar( 6, new OCRPoint(3, 4, 0x8D8F8D, 6), new OCRPoint(4, 7, 0xCACBCA, 6), new OCRPoint(1, 1, 0xFBFBFB, 6)) },
			{ 'C', new OCRChar( 6, new OCRPoint(1, 1, 0xE8E8E8, 6), new OCRPoint(2, 2, 0xC9CAC9, 6), new OCRPoint(5, 3, 0xDBDBDB, 6)) },
			{ 'c', new OCRChar( 5, new OCRPoint(3, 2, 0x818481, 6), new OCRPoint(5, 8, 0x949694, 6), new OCRPoint(4, 4, 0xECEDEC, 6))  },
			// Search for 'D' - A bug here (in Autoit ?) made that x was decreased by 2 after checking for this letter. 
			{ 'D', new OCRChar( 6, 5, new OCRPoint(3, 7, 0xACAEAC, 6), new OCRPoint(6, 1, 0xA0A2A0, 6), new OCRPoint(2, 3, 0xB6B8B6, 6))  },
			{ 'd', new OCRChar( 6, new OCRPoint(3, 4, 0x8D8F8D, 6), new OCRPoint(4, 7, 0xE4E5E4, 6), new OCRPoint(1, 3, 0xABADAB, 6)) },
			{ 'E', new OCRChar( 6, new OCRPoint(2, 6, 0xA5A7A5, 6), new OCRPoint(2, 2, 0xC3C5C3, 6), new OCRPoint(3, 0, 0x767976, 6)) },
			{ 'e', new OCRChar( 6, new OCRPoint(1, 2, 0x404440, 6), new OCRPoint(2, 3, 0xF5F5F5, 6), new OCRPoint(4, 6, 0x7B7D7B, 6)) },
			{ 'F', new OCRChar( 5, new OCRPoint(3, 2, 0x515451, 6), new OCRPoint(1, 8, 0xE1E1E1, 6), new OCRPoint(5, 1, 0xC6C7C6, 6)) },
			{ 'f', new OCRChar( 5, new OCRPoint(5, 1, 0xC4C5C4, 6), new OCRPoint(2, 4, 0xBFC0BF, 6), new OCRPoint(2, 8, 0x535653, 6)) },
			{ 'G', new OCRChar( 6, new OCRPoint(4, 4, 0x707370, 6), new OCRPoint(3, 0, 0xC4C5C4, 6), new OCRPoint(4, 5, 0xD1D2D1, 6)) },
			{ 'g', new OCRChar( 5, new OCRPoint(1, 9, 0xD7D8D7, 6), new OCRPoint(4, 9, 0xF2F2F2, 6), new OCRPoint(4, 4, 0xE6E7E6, 6)) },
			{ 'H', new OCRChar( 8, new OCRPoint(5, 8, 0x5F635F, 6), new OCRPoint(3, 3, 0x464A46, 6), new OCRPoint(1, 1, 0xFAFAFA, 6)) },
			{ 'h', new OCRChar( 6, new OCRPoint(2, 5, 0x818481, 6), new OCRPoint(3, 4, 0xD3D4D3, 6), new OCRPoint(5, 3, 0x646764, 6)) },
			{ 'I', new OCRChar( 2, new OCRPoint(1, 2, 0xFEFEFE, 6), new OCRPoint(2, 5, 0xA3A5A3, 6), new OCRPoint(1, 8, 0xE1E1E1, 6)) },
			{ 'i', new OCRChar( 2, new OCRPoint(1, 1, 0xF0F1F0, 6), new OCRPoint(1, 2, 0x454945, 6), new OCRPoint(1, 8, 0xE1E1E1, 6)) },
			{ 'J', new OCRChar( 6, new OCRPoint(1, 5, 0x8E908E, 6), new OCRPoint(4, 7, 0xC4C5C4, 6), new OCRPoint(6, 1, 0xFBFBFB, 6)) },
			{ 'j', new OCRChar( 2, new OCRPoint(1, 0, 0xA0A2A0, 6), new OCRPoint(1, 4, 0xFDFDFD, 6), new OCRPoint(1, 9, 0xFEFEFE, 6)) },
			{ 'K', new OCRChar( 7, new OCRPoint(2, 3, 0xC7C8C7, 6), new OCRPoint(3, 6, 0x828482, 6), new OCRPoint(6, 0, 0x7C7E7C, 6)) },
			{ 'k', new OCRChar( 5, new OCRPoint(1, 1, 0xFBFBFB, 6), new OCRPoint(2, 1, 0x646764, 6), new OCRPoint(4, 8, 0xDBDCDB, 6)) },
			{ 'L', new OCRChar( 4, new OCRPoint(2, 7, 0xFDFDFD, 6), new OCRPoint(4, 7, 0x828482, 6), new OCRPoint(1, 0, 0x646764, 6)) },
			{ 'l', new OCRChar( 2, new OCRPoint(1, 2, 0xFEFEFE, 6), new OCRPoint(0, 2, 0x909290, 6), new OCRPoint(1, 7, 0xFEFEFE, 6)) },
			{ 'M', new OCRChar( 10, new OCRPoint(4, 8, 0x8C8E8C, 6), new OCRPoint(6, 1, 0x6A6D6A, 6), new OCRPoint(8, 8, 0xA9ABA9, 6)) },
			{ 'm', new OCRChar( 10, new OCRPoint(1, 8, 0xD5D6D5, 6), new OCRPoint(7, 2, 0x585C58, 6), new OCRPoint(5, 4, 0xFFFFFF, 6)) },
			{ 'N', new OCRChar( 7, new OCRPoint(3, 1, 0x949694, 6), new OCRPoint(5, 4, 0x707370, 6), new OCRPoint(6, 1, 0xBFC0BF, 6)) },
			{ 'n', new OCRChar( 6, new OCRPoint(5, 3, 0xBEC0BE, 6), new OCRPoint(4, 8, 0x8A8D8A, 6), new OCRPoint(2, 5, 0x7A7D7A, 6)) },
			{ 'O', new OCRChar( 6, new OCRPoint(2, 6, 0xBBBDBB, 6), new OCRPoint(2, 0, 0x888A88, 6), new OCRPoint(5, 6, 0xD9DAD9, 6)) },
			{ 'o', new OCRChar( 6, new OCRPoint(2, 4, 0x888A88, 6), new OCRPoint(4, 5, 0xA9ABA9, 6), new OCRPoint(3, 2, 0x585C58, 6)) },
			{ 'P', new OCRChar( 6, new OCRPoint(4, 2, 0x636663, 6), new OCRPoint(2, 8, 0x858785, 6), new OCRPoint(5, 3, 0xE6E7E6, 6)) },
			{ 'p', new OCRChar( 5, new OCRPoint(2, 4, 0x8D908D, 6), new OCRPoint(1, 10, 0xFFFFFF, 6), new OCRPoint(5, 3, 0x898B89, 6)) },
			{ 'Q', new OCRChar( 7, new OCRPoint(4, 9, 0xE6E7E6, 6), new OCRPoint(2, 2, 0xABADAB, 6), new OCRPoint(6, 4, 0xFFFFFF, 6)) },
			{ 'q', new OCRChar( 6, new OCRPoint(3, 4, 0x696C69, 6), new OCRPoint(4, 8, 0xFFFFFF, 6), new OCRPoint(4, 10, 0xC1C2C1, 6)) },
			{ 'R', new OCRChar( 7, new OCRPoint(4, 2, 0x575A57, 6), new OCRPoint(3, 6, 0x939593, 6), new OCRPoint(6, 1, 0xB8BAB8, 6)) },
			{ 'r', new OCRChar( 5, new OCRPoint(4, 5, 0x747774, 6), new OCRPoint(2, 6, 0x5F635F, 6), new OCRPoint(2, 2, 0x464A46, 6)) },
			{ 'S', new OCRChar( 7, new OCRPoint(2, 0, 0x8E908E, 6), new OCRPoint(5, 4, 0x9C9E9C, 6), new OCRPoint(3, 7, 0x7C7F7C, 6)) },
			{ 's', new OCRChar( 6, new OCRPoint(2, 2, 0x707370, 6), new OCRPoint(4, 4, 0xC3C4C3, 6), new OCRPoint(2, 7, 0x909290, 6)) },
			// 2 removed from each dx for 'T'
			{ 'T', new OCRChar( 6, 5, new OCRPoint(-1, 0, 0x707370, 6), new OCRPoint(2, 2, 0xB7B8B7, 6),  new OCRPoint(0, 8, 0x969896, 6)) },
			{ 't', new OCRChar( 6, new OCRPoint(2, 1, 0x767976, 6), new OCRPoint(1, 8, 0xB6B8B6, 6), new OCRPoint(4, 6, 0xA6A8A6, 6)) },
			{ 'U', new OCRChar( 7, new OCRPoint(1, 1, 0xFBFBFB, 6), new OCRPoint(1, 8, 0x626562, 6), new OCRPoint(5, 1, 0xBFC0BF, 6)) },
			{ 'u', new OCRChar( 6, new OCRPoint(2, 3, 0x525652, 6), new OCRPoint(2, 7, 0xCBCCCB, 6), new OCRPoint(4, 3, 0x888A88, 6)) },
			{ 'V', new OCRChar( 7, new OCRPoint(2, 2, 0x898C89, 6), new OCRPoint(3, 5, 0x707370, 6), new OCRPoint(5, 7, 0xE1E2E1, 6)) },
			{ 'v', new OCRChar( 6, new OCRPoint(2, 3, 0x595C59, 6), new OCRPoint(3, 6, 0x525652, 6), new OCRPoint(5, 3, 0xEEEEEE, 6)) },
			{ 'W', new OCRChar( 11, new OCRPoint(2, 3, 0xAEB0AE, 6), new OCRPoint(6, 1, 0xC5C6C5, 6), new OCRPoint(9, 8, 0x989A98, 6)) },
			{ 'w', new OCRChar( 9, new OCRPoint(3, 5, 0x8E918E, 6), new OCRPoint(5, 7, 0x747674, 6), new OCRPoint(8, 8, 0x797C79, 6)) },
			{ 'X', new OCRChar( 7, new OCRPoint(1, 2, 0x989B98, 6), new OCRPoint(3, 7, 0x939593, 6), new OCRPoint(5, 4, 0xB6B8B6, 6)) },
			{ 'x', new OCRChar( 6, new OCRPoint(1, 4, 0x8C8E8C, 6), new OCRPoint(2, 8, 0x9EA09E, 6), new OCRPoint(5, 6, 0x4C4F4C, 6)) },
			{ 'Y', new OCRChar( 7, new OCRPoint(2, 1, 0xA1A3A1, 6), new OCRPoint(5, 5, 0x8C8E8C, 6), new OCRPoint(3, 8, 0xD5D6D5, 6)) },
			{ 'y', new OCRChar( 6, new OCRPoint(1, 8, 0x5D615D, 6), new OCRPoint(3, 7, 0xEEEEEE, 6), new OCRPoint(3, 10, 0xE8E9E8, 6)) },
			{ 'Z', new OCRChar( 7, new OCRPoint(2, 2, 0x505450, 6), new OCRPoint(1, 7, 0x707370, 6), new OCRPoint(5, 7, 0x888B88, 6)) },
			{ 'z', new OCRChar( 4, new OCRPoint(1, 4, 0x757775, 6), new OCRPoint(3, 5, 0x757875, 6), new OCRPoint(2, 8, 0xDADBDA, 6)) },
			{ ',', new OCRChar( 3, new OCRPoint(1, 7, 0xE8E8E8, 6), new OCRPoint(1, 8, 0xFEFEFE, 6), new OCRPoint(1, 9, 0xA5A7A5, 6)) },
			{ ' ', new OCRChar( 2, new OCRPoint(1, 3, 0x404440, 6), new OCRPoint(1, 7, 0x404440, 6), new OCRPoint(2, 5, 0x404440, 6)) }
		};
		}
		public static readonly Dictionary<char, OCRChar> OCRData = GetOCRData();
		#endregion OCR Data
	}
}
