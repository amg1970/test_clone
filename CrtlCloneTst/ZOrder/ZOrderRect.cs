using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVisionPlayer.ZOrder
{
  //mm: class that represent an overlapping rect for the zOrder among external windows and internal panels
  class ZOrderRect
  {
    public int _top;
    public int _left;
    public int _right;
    public int _bottom;

    //---------------------------
    public ZOrderRect()
    {
       _top = 0;
       _left = 0;
       _right = 0;
       _bottom = 0;
    }

    //---------------------------
    public ZOrderRect(int left, int top, int right, int bottom)
    {
       setCoordinates(left, top, right, bottom);
    }

    //---------------------------
    public int setCoordinates(int left, int top, int right, int bottom)
    {
      _left   = left;
      _top    = top;
      _right  = right;
      _bottom = bottom;
      return 0;
    }


    //---------------------------
    public int setCoordinatesFromZone(ZOrderClient zOrderClient)
    {
        _left   = zOrderClient._left;
        _top    = zOrderClient._top;
        _right  = zOrderClient._right;
        _bottom = zOrderClient._bottom;
        return 0;
    }

    //----------------------------------------
    public bool isIntersectedWithZone(ZOrderClient zOrderClient)
    {
      //intersectRect = ! (r2.left > r1.right || r2.right < r1.left || r2.top > r1.bottom || r2.bottom < r1.top);
      bool bIntersect = !
                 ((this._left   > zOrderClient._right)  ||
                  (this._right  < zOrderClient._left)   ||
                  (this._top    > zOrderClient._bottom) ||
                  (this._bottom < zOrderClient._top)
                );
      return bIntersect;
    }

    //----------------------------------------
    public bool isZoneInsideRect(ZOrderClient zOrderClient)
    {
      bool bInsideRect = 
                (
                  (zOrderClient._left   >= this._left   )  &&
                  (zOrderClient._right  <= this._right  )  &&
                  (zOrderClient._top    >= this._top    )  &&
                  (zOrderClient._bottom <= this._bottom )  
                );
      return bInsideRect;
    }


    //----------------------------------------
    public int intersectWithZone(ZOrderClient zOrderClient)
    {
      if (!isIntersectedWithZone(zOrderClient))
        return setCoordinates(0, 0, 0, 0);

      int newLeft   = Math.Max(zOrderClient._left, _left);
      int newRight  = Math.Min(zOrderClient._right, _right);

      int newTop    = Math.Max(zOrderClient._top, _top);
      int newBottom = Math.Min(zOrderClient._bottom, _bottom);

      setCoordinates(newLeft, newTop, newRight, newBottom);
      return 0;
    }

    //----------------------------------------
    public int unionWithZone(ZOrderClient zOrderClient)
    {
        int newLeft = Math.Min(zOrderClient._left, _left);
        int newRight = Math.Max(zOrderClient._right, _right);

        int newTop = Math.Min(zOrderClient._top, _top);
        int newBottom = Math.Max(zOrderClient._bottom, _bottom);

        setCoordinates(newLeft, newTop, newRight, newBottom);
        return 0;
    }



  }
}
