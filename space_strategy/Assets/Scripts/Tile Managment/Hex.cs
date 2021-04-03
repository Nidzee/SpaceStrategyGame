using UnityEngine;

public class Hex : MonoBehaviour
{
    public int q_arr_position; // Column position in array
    public int r_arr_position; // Row position in array

    public int Q;              // Column
    public int R;              // Row
    public int S;              // Additional info s=-(q+r)

    public Tile_Type tile_Type;// Tile type of each Hex

    static float WIDTH_MULTIPLIER = Mathf.Sqrt(3) /2;
    static float radius = 1f;
    static float height = radius*2;
    static float width = WIDTH_MULTIPLIER * height;
    static float horiz_offset = width;
    static float vertical_offset = height * 0.75f;


    public void CreateMapTile(int q_arr_position, int r_arr_position)
    {        
        this.q_arr_position = q_arr_position;
        this.r_arr_position = r_arr_position;

        Q = (q_arr_position - r_arr_position / 2);
        R = r_arr_position;
        S = -(Q + R);

        gameObject.name = Q + "." + R + "." + S;

        Position();
    }

    public void Position()
    {
        if (r_arr_position % 2 == 1)
            gameObject.transform.position = new Vector3 (horiz_offset * q_arr_position + horiz_offset/2f,vertical_offset * r_arr_position,0);
        else
            gameObject.transform.position = new Vector3 (horiz_offset * (q_arr_position),vertical_offset * r_arr_position,0);
    }
}
