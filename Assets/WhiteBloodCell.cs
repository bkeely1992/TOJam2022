using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBloodCell : MonoBehaviour
{
    [SerializeField] float maxSpeed = 1.0f;
    [SerializeField] float acceleration = 0.1f;
    [SerializeField] float range = 50.0f;
    [SerializeField] float timeToCheck = 0.5f;
    [SerializeField] List<Sprite> possibleFaces;
    [SerializeField] SpriteRenderer faceSprite;
    [SerializeField] List<GameObject> possibleWeapons;
    

    public enum State
    {
        wait, alert
    }

    private State state = State.wait;
    private float currentSpeed = 0.0f;
    private float timeSinceLastCheck = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        int chosenFace = Random.Range(0, possibleFaces.Count);
        faceSprite.sprite = possibleFaces[chosenFace];
        int chosenWeapon = Random.Range(0, possibleWeapons.Count);
        possibleWeapons[chosenWeapon].SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeSinceLastCheck += Time.deltaTime;
        if(timeSinceLastCheck > timeToCheck)
        {
            if (GameManager.Instance.player)
            {
                if(Vector3.Distance(transform.position,GameManager.Instance.player.transform.position) < range)
                {
                    state = State.alert;
                }
                else
                {
                    state = State.wait;
                }
            }
            timeSinceLastCheck = 0.0f;
        }

        switch (state)
        {
            case State.alert:
                //Home in
                if (GameManager.Instance.player &&
                    !GameManager.Instance.player.isInvincible)
                {
                    transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.player.transform.position, maxSpeed*Time.deltaTime);
                }
                
                break;
            case State.wait:
                //Do nothing

                break;
        }
        
    }
}
