using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstValues
{
    public const string PLAYER_PREFS_ENUM_WEAPON_TAG = "WeaponTag";
    public const string PLAYER_PREFS_ENUM_WEAPON_SKIN_TAG = "WeaponSkinTag";
    public const string PLAYER_PREFS_ENUM_PANT_SKIN_TAG = "PantSkinTag";
    public const string PLAYER_PREFS_BOOL_SOUND_SETTING = "Sound Setting";
    public const string PLAYER_PREFS_BOOL_VIBRATE_SETTING = "Vibrate Setting";
    public const int PLAYER_PREFS_BOOL_TRUE_VALUE = 1;
    public const int PLAYER_PREFS_BOOL_FALSE_VALUE = 0;
    public const string TAG_PLAYER = "Player";
    public const string ANIM_TRIGGER_IDLE = "Idle";
    public const string ANIM_TRIGGER_RUN = "Run";
    public const string ANIM_TRIGGER_ATTACK = "Attack";
    public const string ANIM_TRIGGER_DANCE_WIN = "Dance_Win";
    public const string ANIM_TRIGGER_DANCE_CHAR_SKIN = "Dance_CharSkin";
    public const string ANIM_TRIGGER_DEAD = "Dead";
    public const string ANIM_PLAY_DEFAULT_IDLE = "Default Idle";
    public const float VALUE_BASE_ATTACK_RANGE = 6f;
    public const float VALUE_BASE_ATTACK_RATE = 2f;
    public const float VALUE_WEAPON_DEFAULT_LIFE_TIME = 2f;
    public const float WALUE_WEAPON_DEFAULT_FLY_SPEED = 8f;
    public const float VALUE_PLAYER_ATTACK_ANIM_THROW_TIME_POINT = 0.24f;
    public const float VALUE_PLAYER_ATTACK_ANIM_END_TIME_POINT = 0.64f;
    public const float VALUE_AI_PATROL_RANGE = 25f;
    public const float VALUE_AI_IDLE_MAX_TIME = 3f;
    public const float VALUE_AI_STOP_DIST_THRESHOLD = 2f;
    public const float VALUE_CHARACTER_UP_SIZE_RATIO = 0.1f;
    public const float VALUE_BOT_DEAD_TIME = 2f;
    public const string VALUE_CHARACTER_DEFAULT_NAME = "ABCDE";
    public static Color VALUE_CHARACTER_DEFAULT_COLOR = new Color(119f/255, 231f/255, 84f/255);
    public const int LAYER_MASK_ENEMY = 1 << 6;
}
