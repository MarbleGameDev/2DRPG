using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	public static class GameState {

		public static bool WindowOpen = false;

		public static GameStates CurrentState { get; private set; }

		static GameState(){
			CurrentState = GameStates.Game;
		}

		public static void SetGameState(GameStates state) {
			CurrentState = state;
		}

		public enum GameStates {
			Menus, Game, Paused
		};
	}
}
