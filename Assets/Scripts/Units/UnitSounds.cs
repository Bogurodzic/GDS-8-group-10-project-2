using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSounds : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioClip _deathSound;
    private AudioClip _attackSound;
    private AudioClip _abilitySound;
 
    private void LoadAudio()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }
    
    public void LoadUnitSounds(UnitData unitData)
    {
        LoadAudio();
        _deathSound = unitData.deathClip;
        _attackSound = unitData.attackClip;
        if (unitData.unitAbility && unitData.unitAbility.abilityAudio)
        {
            _abilitySound = unitData.unitAbility.abilityAudio;
        }
    }

    public void PlayAttackSound()
    {
        if (MusicManager.Instance.IsEnabled())
        {
            _audioSource.clip = _attackSound;
            _audioSource.Play();
        }
    }
    
    public void PlayDeathSound()
    {
        if (MusicManager.Instance.IsEnabled())
        {
            _audioSource.clip = _deathSound;
            _audioSource.Play();
        }
    }

    public void PlayAbilitySound()
    {
        if (MusicManager.Instance.IsEnabled())
        {
            _audioSource.clip = _abilitySound;
            _audioSource.Play();
        }
    }
}
