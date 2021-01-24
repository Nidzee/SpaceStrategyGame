using UnityEngine;

public class Hex : MonoBehaviour
{
    public int q_arr_position; // Column position in array
    public int r_arr_position; // Row position in array
    public int Q;              // Column
    public int R;              // Row
    public int S;              // Additional info s=-(q+r)
    public Tile_Type tile_Type;// Tile type of each Hex

    static readonly float WIDTH_MULTIPLIER =  Mathf.Sqrt(3) /2;
    static readonly float radius = 1f;
    static readonly float height = radius*2;
    static readonly float width = WIDTH_MULTIPLIER * height;
    static readonly float horiz_offset = width;
    static readonly float vertical_offset = height * 0.75f;

    //  with ARRAY-COORDINATES as parametrs
    public void Initialize_with_arr_pos(int q_arr_position, int r_arr_position, Tile_Type tt = Tile_Type.FreeTile)
    {
        //Array coords
        this.q_arr_position = q_arr_position;
        this.r_arr_position = r_arr_position;

        //Cubic coords
        this.Q = (q_arr_position - r_arr_position / 2);
        this.R = r_arr_position;
        this.S = -(this.Q + this.R);

        this.tile_Type = tt;

        //TextMesh tm = GetComponentInChildren<TextMesh>();
        //tm.text = ("["+this.Q + " " + this.R + " " + this.S+"]");
    }

    //  with CUBIC-COORDINATES as parametrs
    public void Initialize_with_cubic_pos(int q,int r,int s, Tile_Type tt = Tile_Type.FreeTile)
    {
        //Array coords
        this.q_arr_position = q + (r/2);
        this.r_arr_position = r;

        //Cubic coords
        this.Q = q;
        this.R = r;
        this.S = s;

        this.tile_Type = tt;

        //TextMesh tm = GetComponentInChildren<TextMesh>();
        //tm.text = ("["+this.Q + " " + this.R + " " + this.S+"]");
    }

    // Positioning HEX as an array members, using information taken from constructor
    public Vector3 Position()
    {
        if (this.r_arr_position % 2 == 1)
            return new Vector3 (horiz_offset * this.q_arr_position + horiz_offset/2f,vertical_offset * this.r_arr_position,0); //horiz_offset * this.Q + horiz_offset/2f,vertical_offset * this.R,0 - for row offset
        else
            return new Vector3 (horiz_offset * (this.q_arr_position),vertical_offset * this.r_arr_position,0);
    }
}
