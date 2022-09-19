using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstValues
{
    public const string TAG_PLAYER = "Player";
    public const string ANIM_TRIGGER_IDLE = "Idle";
    public const string ANIM_TRIGGER_RUN = "Run";
    public const string ANIM_TRIGGER_ATTACK = "Attack";
    public const string ANIM_TRIGGER_DANCE_WIN = "Dance Win";
    public const string ANIM_TRIGGER_DANCE_CHAR_SKIN = "Dance CharSkin";
    public const string ANIM_TRIGGER_DEAD = "Dead";
    public const string ANIM_PLAY_DEFAULT_IDLE = "Default Idle";
    public const string ANIM_TRIGGER_SCORE_POPUP = "Score Popup";
    public const string CINEMACHINE_ANIM_MAIN_MENU = "Main Menu";
    public const string CINEMACHINE_ANIM_PLAYING = "Playing";
    public const string CINEMACHINE_ANIM_SHOPPING = "Shopping";
    public const int VALUE_EXP_PER_LEVEL = 1000;
    public const float VALUE_PERCENTAGE_OF_TRIPLE_REWARD = 72f;
    public const float VALUE_PERCENTAGE_OF_BOT_HAVE_SHIELD = 43f;
    public const float VALUE_BASE_ATTACK_RANGE = 6f;
    public const float VALUE_BASE_ATTACK_RATE = 2f;
    public const float VALUE_DEFAULT_ATTACK_POS_OFFSET = 0.85f;
    public const float VALUE_WEAPON_DEFAULT_LIFE_TIME = 2f;
    public const float WALUE_WEAPON_DEFAULT_FLY_SPEED = 8f;
    public const float VALUE_PLAYER_ATTACK_ANIM_THROW_TIME_POINT = 0.24f;
    public const float VALUE_PLAYER_ATTACK_ANIM_END_TIME_POINT = 0.64f;
    public const float VALUE_AI_PATROL_RANGE = 25f;
    public const float VALUE_AI_IDLE_MAX_TIME = 3f;
    public const float VALUE_AI_STOP_DIST_THRESHOLD = 2f;
    public const float VALUE_CHARACTER_UP_SIZE_RATIO = 0.1f;
    public const float VALUE_BOT_DEAD_TIME = 2f;
    public const float VALUE_SCORE_POPUP_TEXT_ANIMATION_TIME = 0.48f;
    public const string VALUE_CHARACTER_DEFAULT_NAME = "ABCDE";
    public static Color VALUE_CHARACTER_DEFAULT_COLOR = new Color(119f / 255, 231f / 255, 84f / 255);
    public static Color VALUE_UI_COLOR_FOR_SKIN_SET_4 = new Color(244f / 255, 67f / 255, 54f / 255);
    public static Color VALUE_UI_COLOR_FOR_SKIN_SET_5 = new Color(255f / 255, 235f / 255, 59f / 255);
    public static Quaternion VALUE_PARTICLE_UPGRADE_DEFAULT_ROTATION = Quaternion.Euler(-90f, 0, 0);
    public static Vector3 VALUE_PARTICLE_UPGRADE_DEFAULT_LOCAL_SCALE = Vector3.one * 2.5f;
    public const int LAYER_MASK_ENEMY = 1 << 6;
}
