using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum E_ITEMRANK
    {
        NORMAL,
        RARE,
        LEGEND,
        END
    }

    public enum E_LAYER
    {
        Character = 8,
        Interactive,
        Ground,
        GroundItem,
        Dead,
        Victory,
        END
    }

    public enum E_EFFECT
    {
        NONE,
        RunEffect,
        DeadEffect,
        GetItem,
        END
    }


    public enum E_UIEVENT
    {
        NONE,
        BEGIN_DRAG,
        DRAG,
        EXIT_DRAG,
        CLICK,
        DOWN,
        UP,
        END
    }


    public enum E_CAMERA_TYPE
    {
        NONE,
        QUATER_VIEW,
        SHOP_VIEW,
        ONLY_UI,
        END
    }

    public enum E_SCENE_UI_TYPE
    {
        NONE,
        TITLE,
        IN_GAME,
        SHOP,
        DEAD,
        VICTORY

    }

    public enum E_SCENE_TYPE
    {
        NONE,
        TITLE,
        IN_GAME
    }

    public enum E_SFX
    {
        NONE,
        DAMAGE,
        DEAD,
        END
    }

    public enum E_BGM
    {
        NONE,
        TITLE,
        IN_GAME1,
        END
    }


    public enum E_AVATA_TYPE
    {
        NONE,
        TYPE1,
        TYPE2,
        TYPE3,
        TYPE4,
        TYPE5,
        END
    }

    public enum E_ITEMCATEGORY
    {
        HAIR,
        PARTS,
        COSTUME,
        CONSUME,
        END
    }

    public enum E_COSTUME_TYPE
    {
        NONE,
        HAIR = 0,
        CHEST,
        BOTTOM,
        ARM,
        FOOT,
        COSTUME,
        HAIRCOLOR,
        SKINCOLOR,
        HAND,
        END,
    }

