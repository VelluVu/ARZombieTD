using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class BuildSpot : MonoBehaviour {

    public List<GameObject> buildings = new List<GameObject>();
    public GameObject selectedBuilding;
    public int[] prices;
    int price = 50;
    bool hasGunT;
    bool hasSlowT;
    bool hasMissileT;

    private void Update()
    {
        if (!IsPointerOverUIObject())
        {
            SelectedPosition();
        }
    }

    public void SetBuilding(int index)
    {
        selectedBuilding = buildings[index];
        price = prices[index];
    }

    private void SelectedPosition()
    {

        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out hit, 5000f))
                {
                    if (hit.collider.CompareTag("Ground") && GameControls.euros >= price)
                    {

                        GameObject turret = Instantiate(selectedBuilding, hit.point, Quaternion.identity) as GameObject;
                        turret.transform.SetParent(hit.transform);
                        turret.transform.rotation = turret.transform.parent.transform.rotation;
                        GameControls.euros -= price;
                        selectedBuilding = null;

                        if (turret.GetComponent<GunTurret>().turretType == 3)
                        {
                            GameControls.UnlockAchievement(GPGSIds.achievement_world_thanks_for_the_new_nuke);
                        }
                        if (turret.GetComponent<GunTurret>().turretType == 2)
                        {
                            GameControls.UnlockAchievement(GPGSIds.achievement_not_so_fast_anymore);
                            hasSlowT = true;
                        }            
                        if (turret.GetComponent<GunTurret>().turretType == 0)
                        {
                            GameControls.IncrementAchievement(GPGSIds.achievement_gunner, 1);
                            hasGunT = true;
                        }
                        if(turret.GetComponent<GunTurret>().turretType == 1)
                        {
                            GameControls.IncrementAchievement(GPGSIds.achievement_missiles_op, 1);
                            hasMissileT = true;
                        }                       
                        if(hasSlowT && hasGunT && hasMissileT)
                        {
                            GameControls.UnlockAchievement(GPGSIds.achievement_oh_theres_tactics_in_this_game);
                        }             
                    }
                }
            }
        }
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
