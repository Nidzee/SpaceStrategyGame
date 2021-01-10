using UnityEngine;

public class SelectTileState : ITouchState
{
    private float touchTime = 0.2f;

    public ITouchState DoState()
    {
        DomyState();

        if (GameHendler.Instance.isZooming) // No need for ResetState cuz whole functionality is in *gh.resetInfo();*
        {
            GameHendler.Instance.resetInfo();
            touchTime = 0.2f;
            return GameHendler.Instance.zoomState;
        }

        else if (!GameHendler.Instance.isTileselectState) // This case is executed only after ButtonUp -> reset is already executed
        {
            StateReset(); // Продубльоване виконання функції (тільки для симетрії)
            return GameHendler.Instance.idleState;
        }

        else if (GameHendler.Instance.touchStart != GameHendler.Instance._worldMousePosition) // We moved mouse - CameraMovementState
        {
            StateReset();
            GameHendler.Instance.isCameraState = true;
            return GameHendler.Instance.cameraMovementState;
        }
        
        else
            return GameHendler.Instance.selectTileState;
    }
    
    private void DomyState()
    {
        if (Input.touchCount == 2) // Detects second Touch
        {
            GameHendler.Instance.isZooming = true;
            return;
        }

        if (Input.GetMouseButton(0)) // Timer running
        {
            touchTime -= Time.deltaTime;
            if (touchTime < 0)
                touchTime = 0f;
        }

        else if (Input.GetMouseButtonUp(0)) // End of the state
        {
            if (touchTime > 0 && GameHendler.Instance.touchStart == GameHendler.Instance._worldMousePosition) // if all conditions are correct -> set Selected Tile
            {
                //setCurrentHex(); // work without it and without checking for existing CurrentHex
                GameHendler.Instance.hexColor = GameHendler.Instance.CurrentHex.GetComponent<SpriteRenderer>().color; // TEMP
                GameHendler.Instance.SelectedHex = GameHendler.Instance.CurrentHex;
                GameHendler.Instance.SelectedHex.GetComponent<SpriteRenderer>().color = Color.yellow; // TEMP
            }
            StateReset();
        }
    }

    private void StateReset() // Reset all needed variables before state changing
    {
        GameHendler.Instance.isTileselectState = false;
        GameHendler.Instance.isFirstCollide = false;
        GameHendler.Instance.CurrentHex = null; // if we remove it from here - idle will go back to this state twice
        touchTime = 0.2f;
    }

    public void setCurrentHex()
    {
        GameHendler.Instance.c = pixel_to_pointy_hex(GameHendler.Instance.redPoint.transform.position.x, GameHendler.Instance.redPoint.transform.position.y);
        GameHendler.Instance.CurrentHex = GameObject.Find(GameHendler.Instance.c.q + "." + GameHendler.Instance.c.r + "." + GameHendler.Instance.c.s);
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
