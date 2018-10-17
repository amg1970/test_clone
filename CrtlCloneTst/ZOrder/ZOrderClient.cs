using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MVisionPlayer.Views;

namespace MVisionPlayer.ZOrder
{
    //mm: class that contains the zone information for the zOrder among external windows and internal panels
    public class ZOrderClient
    {
        #region data members

        public int _id;
        public Guid _guid;

        public int _left;
        public int _top;
        public int _right;
        public int _bottom;

        public int _iZOrder;
        public int _iApparitionOrder;

        public IntPtr _windowHandle;
        public IntPtr _parentWindowHandle;

        public Control _control;
        public Control _parent;
        public Control _parentParent;
        public Control _parentParentParent;

        public RenderingContainer _renderingContainer;

        public int _urlType;
        public int _previousUrlType;

        public const int UrlType_HtmlChromiumWPF = 5;
        public const int UrlType_NotSet       = 0;

        #endregion

        #region constructor 

        //---------------- constructor 1
        public ZOrderClient()
        {
            _id = 0;
            _guid = new Guid();

            _left = 0;
            _top = 0;
            _right = 0;
            _bottom = 0;

            _iZOrder = 0;
            _iApparitionOrder = 0;

            _windowHandle = IntPtr.Zero;
            _parentWindowHandle = IntPtr.Zero;

            _control = null;
            _parent = null;
            _parentParent = null;
            _parentParentParent = null;

            _renderingContainer = null;

            _urlType         = ZOrderClient.UrlType_NotSet;
            _previousUrlType = ZOrderClient.UrlType_NotSet;

        }

        //--------------- constructor 2
        public ZOrderClient(int id, Guid guid,
                            int left, int top, int right, int bottom,
                            int iZOrder, int iApparitionOrder,
                            IntPtr windowHandle,
                            IntPtr parentWindowHandle
                           )
        {
            _id = id;
            _guid = guid;

            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;

            _iZOrder = iZOrder;
            _iApparitionOrder = iApparitionOrder;

            _windowHandle = windowHandle;
            _parentWindowHandle = parentWindowHandle;

            _control = null;
            _parent = null;
            _parentParent = null;
            _parentParentParent = null;

            _renderingContainer = null;

            _urlType         = ZOrderClient.UrlType_NotSet;
            _previousUrlType = ZOrderClient.UrlType_NotSet;
        }

        #endregion

        #region public methods

        //------------------------------------
        public bool isSetWithCoordinates()
        {
          if ( (_left==0 ) && (_top == 0) && (_right==0) && (_bottom==0))
            return false; 
          return true;
        }

        //---------------------------------------
        public int copy(ZOrderClient zOrderClientSrc)
        {
            _id = zOrderClientSrc._id;
            _guid = zOrderClientSrc._guid;

            _left = zOrderClientSrc._left;
            _top = zOrderClientSrc._top;
            _right = zOrderClientSrc._right;
            _bottom = zOrderClientSrc._bottom;

            _iZOrder = zOrderClientSrc._iZOrder;
            _iApparitionOrder = zOrderClientSrc._iApparitionOrder;

            _windowHandle = zOrderClientSrc._windowHandle;
            _parentWindowHandle = zOrderClientSrc._parentWindowHandle;

            _control            = zOrderClientSrc._control;
            _parent             = zOrderClientSrc._parent;
            _parentParent       = zOrderClientSrc._parentParent;
            _parentParentParent = zOrderClientSrc._parentParentParent;

            _renderingContainer = zOrderClientSrc._renderingContainer;

            _urlType         = zOrderClientSrc._urlType;
            _previousUrlType = zOrderClientSrc._previousUrlType;

            //_clonePB = zOrderClientSrc._clonePB;
            return 0;
        }

        #endregion

    }
}
