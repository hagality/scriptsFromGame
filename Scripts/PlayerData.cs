using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public long id;
    public Vector3 pos;
    public byte speed;
    public string nick;
    public PlayerUnity playerUnity;

    public PlayerData target;
    public Inventory inventory;

    public short currentHealth;
    public short maxHealth;

    public short currentMana;
    public short maxMana;
    public bool dead;
    public byte dir;
    public byte currentOutfitId;

    public PlayerData(long id, Vector3 pos, byte speed, string nick, PlayerUnity playerUnity, short currentHealth, short maxHealth, short currentMana, short maxMana, bool dead, byte currentOutfitId)
    {
        this.id = id;
        this.pos = pos;
        this.playerUnity = playerUnity;
        this.speed = speed;
        this.nick = nick;
        inventory = new Inventory();

        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;

        this.currentMana = currentMana;
        this.maxMana = maxMana;
        this.dead = dead;

        this.currentOutfitId = currentOutfitId;
    }


    public void ManaGain(short dmg)
    {
        if (currentMana + dmg >= maxMana)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana += dmg;
        }

        if (playerUnity.isMine)
        {
            SetHealthAndManaOnGUI();
        }
    }
    public void DamageGain(short dmg, bool onManaDmg)
    {
        if (dmg > 0)
        {
            if (currentHealth + dmg >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += dmg;
            }
        }
        else
        {
            if (onManaDmg == false)
            {

                currentHealth += dmg;
                PoolingObjManager.instance.GetTextPooling(dmg, false, playerUnity.model.gameObject, playerUnity.transform);
            }
            else
            {


                if (currentMana + dmg >= 0)
                {
                    currentMana += dmg;
                    PoolingObjManager.instance.GetTextPooling(dmg, true, playerUnity.model.gameObject, playerUnity.transform);
                }
                else
                {
                    int offset = Mathf.Abs(currentMana + dmg);

                    currentHealth -= (short)offset;



                    PoolingObjManager.instance.GetTextPooling((short)currentMana, true, playerUnity.model.gameObject, playerUnity.transform);
                    PoolingObjManager.instance.GetTextPooling((short)offset, false, playerUnity.model.gameObject, playerUnity.transform);

                    currentMana = 0;

                }
            }

        }

        if (currentHealth <= 0)
        {
            SetDeath();
        }

        playerUnity.currentHealthImg.fillAmount = ((float)currentHealth / (float)maxHealth);
        //playerUnity.nickText.text = currentHealth.ToString();

        if (playerUnity.isMine)
        {
            SetHealthAndManaOnGUI();
        }
    }

    private void SetDeath()
    {
        currentHealth = 0;
        dead = true;
        playerUnity.animator.Play("death");
        playerUnity.playerCanvas.SetActive(false);

        if (playerUnity.isMine)
        {
            GUIHandler.instance.panelYouAreDeath.SetActive(true);
        }
    }

    public void SetHealth()
    {

        playerUnity.currentHealthImg.fillAmount = ((float)currentHealth / (float)maxHealth);
    }

    public void SetHealthAndManaOnGUI()
    {
        if (playerUnity.isMine)
        {
            GUIHandler.instance.imgHealthInRightPanel.fillAmount = ((float)currentHealth / (float)maxHealth);
            GUIHandler.instance.imgManaInRightPanel.fillAmount = ((float)currentMana / (float)maxMana);

            GUIHandler.instance.txtCurrentHealthInRightPanel.text = currentHealth.ToString();
            GUIHandler.instance.txtCurrentManaInRightPanel.text = currentMana.ToString();
        }
    }

}
