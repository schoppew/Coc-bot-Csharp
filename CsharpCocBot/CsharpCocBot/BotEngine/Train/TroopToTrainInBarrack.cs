using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Data;

namespace CoC.Bot.BotEngine.Train
{
	public class TroopToTrainInBarrack
	{
		
		public TroopToTrainInBarrack(BuildingPointModel model)
		{
			BarrackInfo = model;
		}

		public bool IsValid
		{
			get
			{
				return BarrackInfo != null && (BarrackInfo.BuildingType == BuildingType.DarkBarrack || BarrackInfo.BuildingType == BuildingType.Barrack) && !BarrackInfo.Coordinates.IsEmptyOrZero;
			}
		}
		public BuildingPointModel BarrackInfo { get; private set; }
		public bool IsDark { get { return IsValid && BarrackInfo.BuildingType == BuildingType.DarkBarrack; } }
		public bool IsNormal { get { return IsValid && BarrackInfo.BuildingType == BuildingType.Barrack; } }
		public List<TrainingElement> ToDo;
		public ClickablePoint Location
		{
			get
			{
				return (ClickablePoint)BarrackInfo.Coordinates;
			}
		}

		int? _cumulTime = 0;
		/// <summary>
		/// Return the number of second needed to train all troops 
		/// </summary>			
		public int CumulTime
		{
			get
			{
				if (_cumulTime != null) return _cumulTime.Value;
				_cumulTime = ToDo.Sum(te => te.Troop.TimeToTrain() * te.TotalCount);
				return _cumulTime.Value;
			}
		}


		public TrainingElement Pop()
		{
			TrainingElement first = ToDo.FirstOrDefault(te => te.TotalCount > te.TrainingStarted);
			if (first != null)
			{
				//first.TrainingStarted++;
				return first;
			}
			return null;
		}
		public void Push(Troop troop, int nb = 1)
		{
			_cumulTime = null;
			TrainingElement first = ToDo.FirstOrDefault(te => te.Troop == troop);
			if (first == null)
				ToDo.Add(new TrainingElement(troop, nb));
			else
				first.TotalCount += nb;
		}

	}

}
