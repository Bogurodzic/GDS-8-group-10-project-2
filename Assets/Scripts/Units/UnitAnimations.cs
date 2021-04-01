using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class UnitAnimations : MonoBehaviour
{
    private SkeletonAnimation _skeletonAnimation;
    private SpriteRenderer _sprite;
    private bool _blockAnimation = false;
    private UnitData _unitData;
    private UnitStatistics _unitStatistics;
    void Start()
    {
        LoadSprite();
    }

    void Update()
    {
        
    }
    
    private void LoadSprite()
    {
        _skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
    }

    public void MakeUnitTransparent()
    {
        if (!_blockAnimation)
        {
            _skeletonAnimation.skeleton.A = 0.5f;
        }
        else
        {
            if (!_sprite)
            {
                _sprite = GetComponent<SpriteRenderer>();
            }

            _sprite.color = Color.gray;
        }
    }
    
    public void ReloadSprite(UnitData unitData, UnitStatistics unitStatistics)
    {
        _unitData = unitData;
        _unitStatistics = unitStatistics;
        if (!_blockAnimation)
        {
            if (!_skeletonAnimation)
            {
                LoadSprite();
            }

            _skeletonAnimation.skeletonDataAsset = unitData.skeletonDataAsset;

            if (unitStatistics.team == 1)
            {
                switch (unitData.unitName)
                {
                    case "Archer":
                        _skeletonAnimation.initialSkinName = "ARCHER BLUE";
                        break;
                    case "Boss":
                        _skeletonAnimation.initialSkinName = "BOSS BLUE";
                        break;
                    case "Infantry":
                        _skeletonAnimation.initialSkinName = "SWORDMAN BLUE";
                        break;
                    case "Mage":
                        _skeletonAnimation.initialSkinName = "WIZARD BLUE";
                        break;
                    case "Medic":
                        _skeletonAnimation.initialSkinName = "MEDIC BLUE";
                        break;
                    case "Rogue":
                        _skeletonAnimation.initialSkinName = "ROGUE BLUE";
                        break;
                    case "Spearman":
                        _skeletonAnimation.initialSkinName = "SPEARMAN BLUE";
                        break;
                }

            }
            else
            {
                _skeletonAnimation.initialFlipX = true;
                switch (unitData.unitName)
                {
                    case "Archer":
                        _skeletonAnimation.initialSkinName = "ARCHER RED";
                        break;
                    case "Boss":
                        _skeletonAnimation.initialSkinName = "BOSS RED";
                        break;
                    case "Infantry":
                        _skeletonAnimation.initialSkinName = "SWORDMAN RED";
                        break;
                    case "Mage":
                        _skeletonAnimation.initialSkinName = "WIZARD RED";
                        break;
                    case "Medic":
                        _skeletonAnimation.initialSkinName = "MEDIC RED";
                        break;
                    case "Rogue":
                        _skeletonAnimation.initialSkinName = "ROGUE RED";
                        break;
                    case "Spearman":
                        _skeletonAnimation.initialSkinName = "SPEARMAN RED";
                        break;
                }
            }

            _skeletonAnimation.Initialize(true);
            _skeletonAnimation.Skeleton.SetSkin(_skeletonAnimation.initialSkinName);
            _skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            _skeletonAnimation.AnimationState.Apply(_skeletonAnimation.Skeleton);
            _skeletonAnimation.AnimationState.SetAnimation(0, "IDLE", true);
        }
    }

    public void FaceAnimationToRight()
    {
        if (!_blockAnimation)
        {
            if (_skeletonAnimation.initialFlipX)
            {
                _skeletonAnimation.initialFlipX = false;
                _skeletonAnimation.Initialize(true);
                _skeletonAnimation.Skeleton.SetSkin(_skeletonAnimation.initialSkinName);
                _skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                _skeletonAnimation.AnimationState.Apply(_skeletonAnimation.Skeleton);
                _skeletonAnimation.AnimationState.SetAnimation(0, "WALK", true);
            }
        }
    }
    
    public void FaceAnimationToLeft()
    {
        if (!_blockAnimation)
        {
            if (!_skeletonAnimation.initialFlipX)
            {
                _skeletonAnimation.initialFlipX = true;
                _skeletonAnimation.Initialize(true);
                _skeletonAnimation.Skeleton.SetSkin(_skeletonAnimation.initialSkinName);
                _skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                _skeletonAnimation.AnimationState.Apply(_skeletonAnimation.Skeleton);
                _skeletonAnimation.AnimationState.SetAnimation(0, "WALK", true);
            }
        }
    }



    public void RemoveUnitTransparency()
    {
        if (!_blockAnimation)
        {
            _skeletonAnimation.skeleton.A = 1f;
        }
        else
        {
            if (!_sprite)
            {
                _sprite = GetComponent<SpriteRenderer>();
            }
            _sprite.color = Color.white;
        }
    }
    
    
    public void AnimateOnce(string animationName)
    {
        if (!_blockAnimation)
        {
            _skeletonAnimation.AnimationState.SetAnimation(0, animationName, false);
        }
    }

    public void AnimateUnit(string animationName)
    {
        if (!_blockAnimation)
        {
            _skeletonAnimation.AnimationState.SetAnimation(0, animationName, false);
            _skeletonAnimation.AnimationState.AddAnimation(0, "IDLE", true, 0f);
        }
    }

    public void AnimateLoopUnit(string animationName)
    {
        if (!_blockAnimation)
        {
            _skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
        }
    }

    public void AnimateIdle()
    {
        if (!_blockAnimation)
        {
            _skeletonAnimation.AnimationState.SetAnimation(0, "IDLE", true);
        }
    }
    
    public SkeletonAnimation GetSkeletonAnimation()
    {
        return _skeletonAnimation;
    }

    public void BlockAnimations()
    {
        _blockAnimation = true;
    }
    
    
}
