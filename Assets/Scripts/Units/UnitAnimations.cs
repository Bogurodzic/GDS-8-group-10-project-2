﻿using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class UnitAnimations : MonoBehaviour
{
    private SkeletonAnimation _skeletonAnimation;
    private SpriteRenderer _sprite;
    void Start()
    {
        LoadSprite();
    }

    void Update()
    {
        
    }
    
    private void LoadSprite()
    {
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        _skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
    }

    public void MakeUnitTransparent()
    {
        _skeletonAnimation.skeleton.A = 0.5f;
    }
    
    public void ReloadSprite(UnitData unitData, UnitStatistics unitStatistics)
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
        _skeletonAnimation.AnimationState.Apply( _skeletonAnimation.Skeleton);
        _skeletonAnimation.AnimationState.SetAnimation(0, "IDLE", true);
    }

    public void FaceAnimationToRight()
    {
        if (_skeletonAnimation.initialFlipX)
        {
            _skeletonAnimation.initialFlipX = false;
            _skeletonAnimation.Initialize(true); 
            _skeletonAnimation.Skeleton.SetSkin(_skeletonAnimation.initialSkinName);
            _skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            _skeletonAnimation.AnimationState.Apply( _skeletonAnimation.Skeleton);
            _skeletonAnimation.AnimationState.SetAnimation(0, "WALK", true);  
        }
    }
    
    public void FaceAnimationToLeft()
    {
        if (!_skeletonAnimation.initialFlipX)
        {
            _skeletonAnimation.initialFlipX = true;
            _skeletonAnimation.Initialize(true); 
            _skeletonAnimation.Skeleton.SetSkin(_skeletonAnimation.initialSkinName);
            _skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            _skeletonAnimation.AnimationState.Apply( _skeletonAnimation.Skeleton);
            _skeletonAnimation.AnimationState.SetAnimation(0, "WALK", true);  
        }
    }



    public void RemoveUnitTransparency()
    {
        _skeletonAnimation.skeleton.A = 1f;
    }
    
    
    public void AnimateOnce(string animationName)
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, animationName, false);
    }

    public void AnimateUnit(string animationName)
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, animationName, false);
        _skeletonAnimation.AnimationState.AddAnimation(0, "IDLE", true, 0f);
    }

    public void AnimateLoopUnit(string animationName)
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
    }

    public void AnimateIdle()
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, "IDLE", true);
    }
    
    public SkeletonAnimation GetSkeletonAnimation()
    {
        return _skeletonAnimation;
    }
    
    
}