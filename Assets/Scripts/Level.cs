using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float BORDER_HEIGHT = 26.2f;
    private const float BORDER_Y_DESTROY = -8f;

    public float ySpeed, xSpeed;
    
    // Ground movement variables
    private Vector2 offset;
    private Material groundMaterial;

    // General Assets
    private GameAssets assets;

    private void Start() {
        assets = GameAssets.GetInstance();

        groundMaterial = assets.ground.GetComponent<Renderer>().material;
    }

    private void Update() {
        MoveGround();
        borderMove(assets.leftBorderList);
        borderMove(assets.rightBorderList);
    }

    private void MoveGround() {
        /*
         * Gives the ground a moving animation by changing the offset of the material
         */
        offset = new Vector2(xSpeed, ySpeed);
        groundMaterial.mainTextureOffset += offset * Time.deltaTime;
    }

    private void borderMove(List<Transform> borderList) {
        /* 
         * Moves the borders downwards at a fixed speed, when the border passes a certain high it is moved over the top border
         */
        
        float yTopBorder = -100f;

        foreach (Transform border in borderList)
        {
            border.position += new Vector3(0, -1, 0) * ySpeed * Time.deltaTime;

            if(border.position.y < BORDER_Y_DESTROY) {
                for (int i = 0; i < borderList.Count; i++)
                {
                    if(borderList[i].position.y > yTopBorder){
                        yTopBorder = borderList[i].position.y;
                    }   
                }
                
                border.position = new Vector3(border.position.x, yTopBorder + (BORDER_HEIGHT * .64f), border.position.z);
            }
        }
    }
}
