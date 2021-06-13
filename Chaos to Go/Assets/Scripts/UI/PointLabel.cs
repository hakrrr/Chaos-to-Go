using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLabel : MonoBehaviour
{
    public int points;
    
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 travelDirection;
    [SerializeField]
    private float travelDist;
    [SerializeField]
    private Color positiveCol;
    [SerializeField]
    private Color negativeCol;
    [SerializeField]
    private TextMesh textMesh;

    private float alphaReduction;


    public static void SpawnAt(GameObject prefab, Transform transform, Vector3 position, int points)
    {
        GameObject obj = Instantiate(prefab);
        PointLabel label = obj.GetComponent<PointLabel>();
        label.points = points;
        label.transform.parent = transform;
        label.transform.position = position;
    }


    // Start is called before the first frame update
    void Start()
    {
        textMesh.color = points < 0 ? negativeCol : positiveCol;
        textMesh.text = points < 0 ? "- " : "+ ";
        textMesh.text += Mathf.Abs(points);
        alphaReduction = (speed / travelDist);
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.Normalize(travelDirection));
        travelDist -= Mathf.Abs(speed * Time.deltaTime);
        Color nextCol = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, textMesh.color.a - (alphaReduction * Time.deltaTime));
        textMesh.color = nextCol;
        if(travelDist < 0.0)
        {
            Destroy(gameObject);
        }
    }
}
