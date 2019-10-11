using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PoolingObjManager : MonoBehaviour
{

    public TextPoolingObject prefabTextPoolingObject;
    TextPoolingObject[] textPoolingObjectsArray;


    public GameObject prefabUhPoolingObject;
    GameObject[] UhPoolingObjectArray;


    public static PoolingObjManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeTextPoolingObjects();
        InitializeUhPoolingObjects();
    }


    private void InitializeTextPoolingObjects()
    {
        textPoolingObjectsArray = new TextPoolingObject[length];

        for (int i = 0; i < length; i++)
        {
            TextPoolingObject txt = Instantiate(prefabTextPoolingObject);
            textPoolingObjectsArray[i] = txt;

            txt.transform.SetParent(GUIHandler.instance.textPoolingObjects);
        }
    }



    private void InitializeUhPoolingObjects()
    {
        UhPoolingObjectArray = new GameObject[length];

        for (int i = 0; i < length; i++)
        {
            GameObject uh = Instantiate(prefabUhPoolingObject);
            UhPoolingObjectArray[i] = uh;

            uh.transform.SetParent(GUIHandler.instance.uhPoolingObjects);
            uh.SetActive(false);
        }
    }



    public void GetTextPooling(short damage, bool manaShield, GameObject obj, Transform _transform)
    {
        if (obj.activeSelf == true)
        {
            damage = (short)Mathf.Abs(damage);
            if (damage > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    if (textPoolingObjectsArray[i].active == false)
                    {
                        textPoolingObjectsArray[i].active = true;
                        textPoolingObjectsArray[i].txt.text = damage.ToString();
                        textPoolingObjectsArray[i].transform.position = _transform.position;
                        textPoolingObjectsArray[i].transform.SetParent(_transform.transform);

                        if (manaShield == true)
                        {
                            textPoolingObjectsArray[i].txt.color = Color.blue;
                        }
                        else
                        {
                            textPoolingObjectsArray[i].txt.color = Color.red;
                        }


                        textPoolingObjectsArray[i].gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
    }



    public void GetDefaultTextPooling(string txt, GameObject obj, Transform _transform)
    {
        if (obj.activeSelf == true)
        {
            for (int i = 0; i < length; i++)
            {
                if (textPoolingObjectsArray[i].active == false)
                {
                    textPoolingObjectsArray[i].txt.color = Color.yellow;
                    textPoolingObjectsArray[i].active = true;
                    textPoolingObjectsArray[i].txt.text = txt;
                    textPoolingObjectsArray[i].transform.position = new Vector3(_transform.position.x, _transform.position.y + 1f, _transform.position.z);
                    textPoolingObjectsArray[i].gameObject.SetActive(true);

                    break;
                }
            }

        }
    }



    public void GetUhPooling(Vector3 position, Transform transform)
    {

        for (int i = 0; i < length; i++)
        {
            if (UhPoolingObjectArray[i].gameObject.activeSelf == false)
            {
                if (Vector3.Distance(ClientGameManager.instance.mainPlayer.pos, position) < 10f)
                {
                    UhPoolingObjectArray[i].gameObject.SetActive(true);
                    UhPoolingObjectArray[i].transform.position = position;
                    UhPoolingObjectArray[i].transform.SetParent(transform);
                }
                break;
            }
        }
    }


}

