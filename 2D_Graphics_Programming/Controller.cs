using Microsoft.Xna.Framework; // DEFAULT
using Microsoft.Xna.Framework.Input; // DEFAULT

namespace _2D_Graphics_Programming
{
    class Controller
    {
        public enum Type { Keyboard, GamePad };
        public enum Action { NONE = -1, MOVE_RIGHT, MOVE_DOWN, MOVE_LEFT, MOVE_UP };
        public Type type;

        public class InputState
        {
            public Action action { get; set; }
            public float jerk { get; set; }
        }
        InputState state;

        public Controller()
        {
            type = Type.Keyboard;
            state = new InputState();
        }

        public InputState Update()
        {
            if (GamePad.GetState(PlayerIndex.One).IsConnected == true)
                type = Type.GamePad;
            else
                type = Type.Keyboard;

            return Input();
        }

        public InputState Input()
        {
            state.action = Action.NONE;
            state.jerk = 0.0f;

            if (type == Type.Keyboard)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    state.action = Action.MOVE_RIGHT;
                    state.jerk = 1.0f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    state.action = Action.MOVE_LEFT;
                    state.jerk = -1.0f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    state.action = Action.MOVE_DOWN;
                    state.jerk = 1.0f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    state.action = Action.MOVE_UP;
                    state.jerk = -1.0f;
                }
            }

            if (type == Type.GamePad)
            {
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadRight))
                {
                    state.action = Action.MOVE_RIGHT;
                    state.jerk = 1.0f;
                }
                else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadLeft))
                {
                    state.action = Action.MOVE_LEFT;
                    state.jerk = -1.0f;
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown))
                {
                    state.action = Action.MOVE_DOWN;
                    state.jerk = 1.0f;
                }
                else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp))
                {
                    state.action = Action.MOVE_UP;
                    state.jerk = -1.0f;
                }

                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickRight))
                {
                    state.action = Action.MOVE_RIGHT;
                    state.jerk = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
                }
                else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickLeft))
                {
                    state.action = Action.MOVE_LEFT;
                    state.jerk = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown))
                {
                    state.action = Action.MOVE_DOWN;
                    state.jerk = -GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;
                }
                else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickUp))
                {
                    state.action = Action.MOVE_UP;
                    state.jerk = -GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;
                }
            }
                return state;
        }
    }
}
