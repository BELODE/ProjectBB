using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveForPhoton : Photon.MonoBehaviour
{
    public float speed = 3;
    Animator ani;
    public PhotonView photonView;
    public GameObject PlayerLight, PlayerCamera, playerCanvas, paletteCamera;
    public Text playerNameText;
    void Awake()
    {
        if (photonView.isMine)
        {
            ani = gameObject.GetComponent<Animator>();

            PlayerLight.SetActive(true);
            PlayerCamera.SetActive(true);
            playerCanvas.SetActive(true);
            playerCanvas.GetComponent<Canvas>().enabled = false;
            playerNameText.text = PhotonNetwork.playerName;
            playerNameText.color = Color.white;

        }
        else
        {
            playerNameText.text = photonView.owner.name;
            playerNameText.color = Color.cyan;
        }

    }

    void Update()
    {
        if (photonView.isMine)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }
            else
            {
                ani.SetBool("walking", false);
                ani.SetBool("breaking", false);
                this.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (playerCanvas.GetComponent<Canvas>().enabled)
                {
                    paletteCamera.SetActive(false);
                    PlayerCamera.SetActive(true);
                    playerCanvas.GetComponent<Canvas>().enabled = false;
                }
                else
                {
                    paletteCamera.SetActive(true);
                    PlayerCamera.SetActive(false);
                    playerCanvas.GetComponent<Canvas>().enabled = true;
                }
            }
        }
    }


    private void Move(float xMove,float yMove)
    {
        ani.SetBool("walking", true);
        ani.SetBool("breaking", true);

        ani.SetFloat("x", xMove);
        ani.SetFloat("y", yMove);

        float x = xMove * (speed) * Time.deltaTime;
        float y = yMove * (speed) * Time.deltaTime;

        this.transform.Translate(new Vector3(x, y, 0));

        //photonView.RPC("MovementForNetwork", PhotonTargets.AllBuffered,x,y);
        //this.GetComponent<Rigidbody2D>().velocity = new Vector3(xMove * (speed), yMove * (speed), 0);
    }

    [PunRPC]
    void MovementForNetwork(float xMove, float yMove)
    {
        //this.GetComponent<Rigidbody2D>().velocity = new Vector3(xMove * (speed), yMove * (speed), 0);
        //this.transform.Translate(xMove * (speed), yMove * (speed), 0);

    }
}
