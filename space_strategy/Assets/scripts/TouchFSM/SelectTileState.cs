using UnityEngine;

public class SelectTileState : ITouchState
{
    public  float TouchTime = 0.2f;

    public ITouchState DoState(GameHendler gh)
    {
        DomyState(gh);

        if (gh.isZooming) // No need for ResetState cuz whole functionality is in *gh.resetInfo();*
            return gh.zoomState;

        else if (gh.isTileselectState) // This case is executed only after ButtonUp -> reset is already executed
            return gh.idleState;

        else if (gh.touchStart != gh._worldMousePosition) // We moved mouse - CameraMovementState
        {
            //Debug.Log("cameraMovementState");

            //gh.isCameraState = false;
            StateReset(gh);
            return gh.cameraMovementState;
        }
        
        else
            return gh.selectTileState;
    }
    
    private void DomyState(GameHendler gh)
    {
        if(Input.touchCount == 2) // Detects second Touch
        {
            gh.isZooming = true;
            TouchTime = 0.2f;
            gh.resetInfo();
            return;
        }

        if (Input.GetMouseButton(0)) // Timer running
        {
            TouchTime -= Time.deltaTime;
            if (TouchTime < 0)
            {
                TouchTime = 0f;
            }
        }

        else if (Input.GetMouseButtonUp(0)) // End of the state
        {
            if (TouchTime > 0 && gh.touchStart == gh._worldMousePosition) // if all conditions are correct -> set Selected Tile
            {
                setCurrentHex(gh);
                if (gh.CurrentHex) // if for some reason we moved mouse
                {
                    gh.hexColor = gh.CurrentHex.GetComponent<SpriteRenderer>().color; // TEMP
                    gh.SelectedHex = gh.CurrentHex;
                    gh.SelectedHex.GetComponent<SpriteRenderer>().color = Color.yellow; // TEMP
                }
            }
            StateReset(gh);
        }
    }

    private void StateReset(GameHendler gh) // Reset all needed variables before state changing
    {
        gh.isTileselectState = true;
            
        gh.isFirstCollide = false;
        gh.CurrentHex = null;
        TouchTime = 0.2f;
    }

    public void setCurrentHex(GameHendler gh)
    {
        gh.c = pixel_to_pointy_hex(gh.redPoint.transform.position.x, gh.redPoint.transform.position.y);
        gh.CurrentHex = GameObject.Find(gh.c.q + "." + gh.c.r + "." + gh.c.s);
    }
    
    
    
    
    private Point pointy_hex_to_pixel(Hex hex)
    {
        var x = (Mathf.Sqrt(3) * hex.Q  +  Mathf.Sqrt(3)/2 * hex.R);
        var y = (                         3f/2 * hex.R);
        return new Point(x, y);
    }

    private Cube pixel_to_pointy_hex(float point_x, float point_y)
    {
        var q = (Mathf.Sqrt(3)/3 * point_x  -  1f/3 * point_y);
        var r = (                              2f/3 * point_y);
        return cube_round(new Cube(q, r, -(q+r)));
    }

    private Cube cube_round(Cube cube)
    {
        var rx = Mathf.Round(cube.q);
        var ry = Mathf.Round(cube.r);
        var rz = Mathf.Round(cube.s);

        var x_diff = Mathf.Abs(rx - cube.q);
        var y_diff = Mathf.Abs(ry - cube.r);
        var z_diff = Mathf.Abs(rz - cube.s);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry-rz;
        else if (y_diff > z_diff)
            ry = -rx-rz;
        else
            rz = -rx-ry;

        return new Cube(rx, ry, rz);
    }
}

public class Point
{
    public float x;
    public float y;

    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Cube
{
    public float q;
    public float r;
    public float s;

    public int c_q_arr_pos;
    public int c_r_arr_pos;
    

    public Cube(float q, float r, float s)
    {
        this.q = q;
        this.r = r;
        this.s = s;

        this.c_q_arr_pos = (int)q + ((int)r/2);
        this.c_r_arr_pos = (int)r;
    }
}
