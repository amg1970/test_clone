using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MVisionPlayer.Views;
//using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Collections;
using System.Runtime.InteropServices;
using MVisionPlayer.Interfaces;

namespace MVisionPlayer.ZOrder
{
    //mm: class that manages the zOrder among external windows and internal panels
    public class ZOrderMng
    {
        #region data members
        //mm
        bool _isShuttingDown = false;

        private int version = 1;

        private IntPtr      _mainWindowHandle;
        private IMainWindow _mainWindow;

        object _zOrderOperatingLock = new object();

        public static int _lastId = 0;

        private List<ZOrderClient> _zOrderList;

        #endregion

        #region constructor 

        //-------------------------------------------------
        //  public members
        //-------------------------------------------------

        //-------------- constructor
        public ZOrderMng()
        {
            _zOrderList = new List<ZOrderClient>();
        }

        #endregion

        #region set get methods

        public static int newId()
        {
            return ++_lastId;
        }

        //--------------------------------
        public void setMainWindow(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        //--------------------------------
        public IMainWindow getMainWindow()
        {
            return _mainWindow;
        }

        //--------------------------------
        public void setMainWindowHandle(IntPtr mainWindowHandle)//this._mainWindow.ParentForm.Handle);
        {
            _mainWindowHandle = mainWindowHandle;
        }

        //--------------------------------
        public IntPtr getMainWindowHandle()//this._mainWindow.ParentForm.Handle);
        {
            return _mainWindowHandle;
        }

        //---------------------- 
        public int setParentWindowHandleByGuid(Guid guid)
        {
            ZOrderClient refClientZone = getClientByGuid(guid);
            if (refClientZone == null)
                return -1;
            ZOrderUtils.SetParent(refClientZone._windowHandle, refClientZone._parentWindowHandle);
            return 0;
        }

        //---------------------- 
        public IntPtr getWHndByGuid(Guid guid)
        {
            ZOrderClient refClientZone = getClientByGuid(guid);
            if (refClientZone == null)
                return (IntPtr)0;
            return refClientZone._windowHandle;
        }

        //---------------------- 
        public int setRenderingContainerByGuid(Guid guid, RenderingContainer renderingContainer)
        {
            ZOrderClient refClientZone = getClientByGuid(guid);
            if (refClientZone == null)
                return -1;
            refClientZone._renderingContainer = renderingContainer;
            return 0;
        }

        //---------------------- 
        public RenderingContainer getRenderingContainerByGuid(Guid guid)
        {
            ZOrderClient refClientZone = getClientByGuid(guid);
            if (refClientZone == null)
                return null;
            return refClientZone._renderingContainer;
        }

        //------------------------------------------------
        public ZOrderClient getClientById(int id)
        {
            ZOrderClient findClientZone = null;
            foreach (ZOrderClient crtClientZone in _zOrderList)
            {
                if (crtClientZone._id == id)
                {
                    findClientZone = crtClientZone;
                    break;
                }
            }
            return findClientZone;
        }

        //------------------------------------------------
        public ZOrderClient getClientByGuid(Guid guid)
        {
            ZOrderClient findClientZone = null;
            foreach (ZOrderClient crtClientZone in _zOrderList)
            {
                if (crtClientZone._guid == guid)
                {
                    findClientZone = crtClientZone;
                    break;
                }
            }
            return findClientZone;
        }

        //------------------------------------------------
        public int setClientByGuid(Guid guid, IntPtr hWnd, IntPtr parentHWnd, int left, int top, int right, int bottom)
        {
            ZOrderClient findClientZone = getClientByGuid(guid);
            if (findClientZone == null)
                return -1;
            findClientZone._windowHandle = hWnd;
            findClientZone._parentWindowHandle = parentHWnd;
            findClientZone._left = left;
            findClientZone._top = top;
            findClientZone._right = right;
            findClientZone._bottom = bottom;
            return 0;
        }

        //------------------------------------------------
        public int setParentsByGuid(Guid guid, Control control, Control parent, Control parentParent, Control parentParentParent, int urlType)
        {
            ZOrderClient findClientZone = getClientByGuid(guid);
            if (findClientZone == null)
                return -1;
            findClientZone._control = control;
            findClientZone._parent = parent;
            findClientZone._parentParent = parentParent;
            findClientZone._parentParentParent = parentParentParent;

            findClientZone._previousUrlType = findClientZone._urlType;
            findClientZone._urlType = urlType;
            return 0;
        }

        //-----------------------------------------------------------
        public int getPreviousUrlTypeByGuid(Guid guid)
        {
            ZOrderClient findClientZone = getClientByGuid(guid);
            if (findClientZone == null)
                return -1;
            return findClientZone._previousUrlType;
        }

        //-----------------------------------------------------------
        public int getUrlTypeByGuid(Guid guid)
        {
            ZOrderClient findClientZone = getClientByGuid(guid);
            if (findClientZone == null)
                return -1;
            return findClientZone._urlType;
        }

        //------------------------------------------------
        public int setClientCoordinatesByGuid(Guid guid, int left, int top, int right, int bottom)
        {
            ZOrderClient findClientZone = getClientByGuid(guid);
            if (findClientZone == null)
                return -1;
            findClientZone._left = left;
            findClientZone._top = top;
            findClientZone._right = right;
            findClientZone._bottom = bottom;
            // temporary for test
            /*
            int a = 1;
            if (findClientZone._iZOrder == 4)
                a = 2; 
            List<ZOrderClient> aboveClientsList = getClientsAboveInZOrder(findClientZone);
            foreach (ZOrderClient zOrderAboveClient in aboveClientsList)
            {
                Console.WriteLine("zone id" + zOrderAboveClient._iZOrder+ " guid " + zOrderAboveClient._guid);
            }*/
            return 0;
        }
        //------------------------------------------------
        public int setClientHWndByGuid(Guid guid, IntPtr hWnd, IntPtr parentHWnd)
        {
            ZOrderClient findClientZone = getClientByGuid(guid);
            if (findClientZone == null)
                return -1;
            findClientZone._windowHandle = hWnd;
            findClientZone._parentWindowHandle = parentHWnd;
            return 0;
        }

        //------------------------------------------------
        public ZOrderClient getClientByHWnd(IntPtr hwnd)
        {
            ZOrderClient findClientZone = null;
            foreach (ZOrderClient crtClientZone in _zOrderList)
            {
                if (crtClientZone._windowHandle == hwnd)
                {
                    findClientZone = crtClientZone;
                    break;
                }
            }
            return findClientZone;
        }

        #endregion



        #region public methods

        //---------------------------------
        public int clearZOrderList()
        {
            _zOrderList.Clear();
            return 0;
        }

        //----------------------------------------------
        public int addZOrderClient(ZOrderClient zOrderClient)
        {
            if (zOrderClient == null)
                return 1;
            ZOrderClient newZOrderClient = new ZOrderClient();
            newZOrderClient.copy(zOrderClient);
            _zOrderList.Add(newZOrderClient);
            return 0;
        }

        //----------------------------------------------
        public int removeZOrderClient(ZOrderClient zOrderClient)
        {
            if (zOrderClient == null)
                return 1;
            _zOrderList.Remove(zOrderClient);
            return 0;
        }

        //----------------------------------------------
        public int removeZOrderClientById(int id)
        {
            ZOrderClient zOrderClient = getClientById(id);
            if (zOrderClient == null)
                return 1;
            _zOrderList.Remove(zOrderClient);
            return 0;
        }

        //----------------------------------------------
        public int removeZOrderClientByHWnd(IntPtr hWnd)
        {
            ZOrderClient zOrderClient = getClientByHWnd(hWnd);
            if (zOrderClient == null)
                return 1;
            _zOrderList.Remove(zOrderClient);
            return 0;
        }

        //----------------------------------------------
        public int removeZOrderClientByGuid(Guid guid)
        {
            ZOrderClient zOrderClient = getClientByGuid(guid);
            if (zOrderClient == null)
                return 1;
            _zOrderList.Remove(zOrderClient);
            return 0;
        }

        //---------------------- 
        public int putClientZoneAndAboveOnScreenByGuid(Guid guid)
        {
            ZOrderClient refClientZone = getClientByGuid(guid);
            if (refClientZone == null)
                return -1;
            return putClientZoneAndAboveOnScreen(refClientZone);
        }


        //---------------------- 
        public int putClientZoneAndAboveOnScreenById(int id)
        {
            ZOrderClient refClientZone = getClientById(id);
            if (refClientZone == null)
                return -1;
            return putClientZoneAndAboveOnScreen(refClientZone);
        }

        //---------------------- 
        public int putClientZoneAndAboveOnScreenByHWnd(IntPtr hWnd)
        {
            ZOrderClient refClientZone = getClientByHWnd(hWnd);
            if (refClientZone == null)
                return -1;
            return putClientZoneAndAboveOnScreen(refClientZone);
        }

        //---------------------- 
        public int putClientZoneOnScreenByGuid(Guid guid)
        {
            ZOrderClient refClientZone = getClientByGuid(guid);
            if (refClientZone == null)
                return -1;
            return putClientZoneOnScreen(refClientZone);
        }

        public void hideAll(ZOrderClient zOrderClientRef, List<ZOrderClient> zOrderClientAboveList)
        {
            if (zOrderClientRef._parent != null)
              zOrderClientRef._parent.Visible = false;
            if (zOrderClientRef._parent.Parent != null)
              zOrderClientRef._parent.Parent.Visible = false;
            foreach (ZOrderClient zOrderAboveClient in zOrderClientAboveList)
            {
                if (zOrderAboveClient._parent != null)
                    zOrderAboveClient._parent.Visible = false;
                if (zOrderAboveClient._parent.Parent != null)
                    zOrderAboveClient._parent.Parent.Visible = false;
            }
        }
        public void showAll(ZOrderClient zOrderClientRef, List<ZOrderClient> zOrderClientAboveList)
        {
            //if (zOrderClientRef._parent != null)
              //  zOrderClientRef._parent.Visible = true;
            //if (zOrderClientRef._parent.Parent != null)
              //  zOrderClientRef._parent.Parent.Visible = true;
            foreach (ZOrderClient zOrderAboveClient in zOrderClientAboveList)
            {
                if (zOrderAboveClient._parent != null)
                    zOrderAboveClient._parent.Visible = true;
                if (zOrderAboveClient._parent.Parent != null)
                    zOrderAboveClient._parent.Parent.Visible = true;
            }
        }


        #endregion 

        #region private methods 

        //-----------------------------------
        //[MethodImpl(MethodImplOptions.Synchronized)]
        private int putClientZoneAndAboveOnScreen(ZOrderClient zOrderRefClient)
        {
            //putClientZoneOnScreen(zOrderRefClient);

            //lock (this) //mm: not necessary. Used Native SetWindowPos with asynchron flag
            {
                if (version == 1) //mm: Do not remove. When/If a hacked on Windows ZOrder will work it will simplify the logic
                {
                    if (true) //mm: Do not remove. When/If a hacked on Windows ZOrder will work it will simplify the logic //hasIntersectedHtmlControls(zOrderRefClient))
                    {
                        //----------------------------------
                        List<ZOrderClient> aboveClientsList = getClientsAboveInZOrder(zOrderRefClient);
                        //hideAll(zOrderRefClient, aboveClientsList);
                        putClientZoneOnScreen(zOrderRefClient);
                        foreach (ZOrderClient zOrderAboveClient in aboveClientsList)
                        {
                            putClientZoneOnScreen(zOrderAboveClient);
                        }
                        //showAll(zOrderRefClient, aboveClientsList);
                    }
                    else
                    {
                        putClientZoneOnPanel(zOrderRefClient);
                    }
                }
                else
                { //version 0
                    putClientZoneOnPanel(zOrderRefClient);
                }
            }
            return 0;
        }

        //------------------------------------------
        private ZOrderRect getOverlappingZOrderRect(ZOrderClient zOrderClientRef)
        {
           ZOrderRect unionRect = new ZOrderRect();
           unionRect.setCoordinatesFromZone(zOrderClientRef);
           //-------------------------------------------
           List<ZOrderClient> aboveClientsList = getClientsAboveInZOrder(zOrderClientRef);
           foreach (ZOrderClient zOrderAboveClient in aboveClientsList)
           {
               if (!zOrderAboveClient.isSetWithCoordinates())
                 continue;
               if (zOrderAboveClient._urlType == 5) // 5 == UrlType_HtmlChromium
                 unionRect.unionWithZone(zOrderAboveClient);
           }
           //-------------------------------------------
           List<ZOrderClient> belowClientsList = getClientsBelowInZOrder(zOrderClientRef);
           foreach (ZOrderClient zOrderBelowClient in belowClientsList)
           {
             if (!zOrderBelowClient.isSetWithCoordinates())
               continue;
             if (zOrderBelowClient._urlType == 5) // 5 == UrlType_HtmlChromium
               unionRect.unionWithZone(zOrderBelowClient);
           }
           return unionRect;
        }

        //---------------------------------------------------------------------------
        /* not-used
        private bool hasAboveOnlyHtmlControls(List<ZOrderClient>  zOrderClientAboveList)
        {
          bool bHasAboveOnlyHtmlControls = false;
          foreach (ZOrderClient zOrderAboveClient in zOrderClientAboveList)
          {
            if ( (zOrderAboveClient._windowHandle != IntPtr.Zero) && (zOrderAboveClient._urlType == ZOrderClient.UrlType_HtmlChromium))
              bHasAboveOnlyHtmlControls = true;
            else
              bHasAboveOnlyHtmlControls = false;
          }
          return bHasAboveOnlyHtmlControls;
        }*/

        //----------------------------------------------------------------
        private bool hasIntersectedHtmlControls(ZOrderClient zOrderClientRef)
        {
          bool b1 = false;
          bool b2 = false;
          bool bIntersectedHtmlControls = false;
          ZOrderRect zOrderOverlapRect = getOverlappingZOrderRect(zOrderClientRef);
          foreach (ZOrderClient zOrderClientOne in this._zOrderList)
          {
            if (zOrderClientOne._urlType != ZOrderClient.UrlType_HtmlChromiumWPF)
              continue;
            if ( ! zOrderClientOne.isSetWithCoordinates())
              continue;
            if (! zOrderOverlapRect.isZoneInsideRect(zOrderClientOne))
              continue;

            if (  (zOrderClientOne._iZOrder == 0) && (zOrderClientOne._left != 0) )
              b1 = true;
            if ((zOrderClientOne._iZOrder == 1) && (zOrderClientOne._left != 0))
              b2 = true;
            //------------------
            foreach (ZOrderClient zOrderClientTwo in this._zOrderList)
            {
              if (zOrderClientTwo._guid == zOrderClientOne._guid)
                continue;
              if (zOrderClientTwo._urlType != ZOrderClient.UrlType_HtmlChromiumWPF)
                continue;
              if (!zOrderClientOne.isSetWithCoordinates())
                  continue;
              if (!zOrderOverlapRect.isZoneInsideRect(zOrderClientTwo))
                continue;

              if (! hasPannelsInBetween(zOrderClientOne, zOrderClientTwo, zOrderOverlapRect))
              {
                bIntersectedHtmlControls = true;
                return bIntersectedHtmlControls;
              }
            }
          } 
          return bIntersectedHtmlControls;
        }

        //------------------------------------------
        private bool hasPannelsInBetween(ZOrderClient zOrderClientOne, ZOrderClient zOrderClientTwo, ZOrderRect zOrderOverlapRect)
        {
          bool bHasPannels = false; 
          ZOrderRect unionRect = new ZOrderRect();
          unionRect.setCoordinatesFromZone(zOrderClientOne);
          unionRect.unionWithZone(zOrderClientTwo);
            //------------------
          foreach (ZOrderClient zOrderOtherClient in this._zOrderList)
          {
            if (zOrderOtherClient._guid == zOrderClientOne._guid)
              continue;
            if (zOrderOtherClient._guid == zOrderClientTwo._guid)
              continue;
            if (!zOrderOtherClient.isSetWithCoordinates())
              continue;
            if (zOrderOtherClient._urlType == ZOrderClient.UrlType_HtmlChromiumWPF)
              continue;
            if (unionRect.isZoneInsideRect(zOrderOtherClient))
            {
              bHasPannels = true;
              break;
            }
              
          }
          return bHasPannels;
        }


        //---------------------------------------------------------------------------
        /* not-used
        private bool hasIntersectedHtmlControls(ZOrderClient zOrderClientRef)
        {
            bool bIntersectedHtmlControls = false;
            int a = 0;
            //ZOrderRect zOrderRect = getOverlappingZOrderRect(zOrderClientRef);
            foreach (ZOrderClient zOrderClient in this._zOrderList)
            {

                    if ((zOrderClientRef._windowHandle != IntPtr.Zero) && (zOrderClientRef._urlType == ZOrderClient.UrlType_HtmlChromium))
                    {
                        if ((zOrderClient._windowHandle != IntPtr.Zero) && (zOrderClient._urlType == ZOrderClient.UrlType_HtmlChromium))
                        {

                            if (zOrderClient._guid == zOrderClientRef._guid)
                                continue;
                            //if ((zOrderClient._left == zOrderClientRef._left) && (zOrderClient._right == zOrderClientRef._right) &&
                              //  (zOrderClient._top == zOrderClientRef._top) && (zOrderClient._bottom == zOrderClientRef._bottom))
                            //{
                            //    continue;
                            //}
                        }
                    }

                    if ((zOrderClientRef._windowHandle != IntPtr.Zero) && (zOrderClientRef._urlType == ZOrderClient.UrlType_HtmlChromium))
                    {
                        if ((zOrderClient._windowHandle != IntPtr.Zero) && (zOrderClient._urlType == ZOrderClient.UrlType_HtmlChromium))
                        {
                            if (isIntersectedWithOtherClient(zOrderClientRef, zOrderClient))
                            {

                                bIntersectedHtmlControls = true;
                                break;
                            }
                        }
                    }
            }
            return bIntersectedHtmlControls;
        }*/


        //---------------------------------------------------------------------------
        // not-used
        private bool hasIntersectedHtmlControlsGlobally()
        {
          bool bIntersectedHtmlControls = false;
          int a = 0;
          foreach (ZOrderClient zOrderClientRef in this._zOrderList)
          {
            foreach (ZOrderClient zOrderClient in this._zOrderList)
            {

              if ((zOrderClientRef._windowHandle != IntPtr.Zero) && (zOrderClientRef._urlType == ZOrderClient.UrlType_HtmlChromiumWPF))
              {
                if ((zOrderClient._windowHandle != IntPtr.Zero) && (zOrderClient._urlType == ZOrderClient.UrlType_HtmlChromiumWPF))
                 {

                     if (zOrderClient._guid == zOrderClientRef._guid)
                            continue;
                     if ((zOrderClient._left == zOrderClientRef._left) && (zOrderClient._right == zOrderClientRef._right) &&
                         (zOrderClient._top == zOrderClientRef._top) && (zOrderClient._bottom == zOrderClientRef._bottom))
                     {
                            if (zOrderClientRef._iZOrder == 1)
                                a = 2;
                            continue;
                     }
                 }
              }
       
              if ((zOrderClientRef._windowHandle != IntPtr.Zero) && (zOrderClientRef._urlType == ZOrderClient.UrlType_HtmlChromiumWPF))
              {
                if ((zOrderClient._windowHandle != IntPtr.Zero) && (zOrderClient._urlType == ZOrderClient.UrlType_HtmlChromiumWPF))
                {
                  if (isIntersectedWithOtherClient(zOrderClientRef, zOrderClient))
                  {
                    if (zOrderClientRef._iZOrder == 1)
                    { 
                       a = 2;
                    }
                    bIntersectedHtmlControls = true;
                    break;
                  }
                }
              }
            }
          }
          return bIntersectedHtmlControls;
        }


        //==============================================================
        // WINDOWS SCENARIO
        //==============================================================

        //-----------------------------------
        private int putClientZoneOnScreen(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;
            if (zOrderRefClient._urlType == 5) //5 = UrlType_HtmlChromium
                return putChromiumClientZoneOnScreen(zOrderRefClient);
            if (zOrderRefClient._urlType == 4) //4 = UrlType_Image
                return putImageClientZoneOnScreen(zOrderRefClient);
            if (zOrderRefClient._urlType == 3) //3 = UrlType_Video
                return putVideoClientZoneOnScreen(zOrderRefClient);
            if (zOrderRefClient._urlType == 2) //2 = UrlType_SWF
                return putSWFClientZoneOnScreen(zOrderRefClient);
            if (zOrderRefClient._urlType == 6) //6 = UrlType_FillColorRect
                return putFillColorRectClientZoneOnScreen(zOrderRefClient);
            if (zOrderRefClient._urlType == 7) //7 = UrlType_NoWPFChromium
                return putNoWPFChromiumClientZoneOnScreen(zOrderRefClient);
            return -1;
        }


        //-----------------------------------
        private int putNoWPFChromiumClientZoneOnScreen(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;
            if (zOrderRefClient._urlType != 7) //7 = UrlType_NoWPFChromium
                return -1;
            ZOrderUtils.SetParent(zOrderRefClient._parentParent.Handle, zOrderRefClient._parentParentParent.Handle);

            ZOrderUtils.SetWindowPos(zOrderRefClient._parentParent.Handle, //zOrderRefClient._parentWindowHandle,
                                      zOrderRefClient._parentParentParent.Handle,                                 //zOrderRefClient._parentParentParent.Handle, //zOrderRefClient._parentWindowHandle, 
                                      zOrderRefClient._left,
                                      zOrderRefClient._top,
                                      zOrderRefClient._right - zOrderRefClient._left, //width, 
                                      zOrderRefClient._bottom - zOrderRefClient._top, //height,
                                      (uint)0x0004 | (uint)0x4000);//| (uint)0x0001 | (uint)0x0002);            
            return 0;
        }


        //-----------------------------------
        private int putFillColorRectClientZoneOnScreen(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;
            if (zOrderRefClient._urlType != 6) //6 = UrlType_FillColorRect
                return -1;
            ZOrderUtils.SetParent(zOrderRefClient._parentParent.Handle, zOrderRefClient._parentParentParent.Handle);

            ZOrderUtils.SetWindowPos(zOrderRefClient._parentParent.Handle, //zOrderRefClient._parentWindowHandle,
                                      zOrderRefClient._parentParentParent.Handle,                                 //zOrderRefClient._parentParentParent.Handle, //zOrderRefClient._parentWindowHandle, 
                                      zOrderRefClient._left,
                                      zOrderRefClient._top,
                                      zOrderRefClient._right - zOrderRefClient._left, //width, 
                                      zOrderRefClient._bottom - zOrderRefClient._top, //height,
                                      (uint)0x0004 | (uint)0x4000);//| (uint)0x0001 | (uint)0x0002);            
            return 0;
        }

        //-----------------------------------
        private int putVideoClientZoneOnScreen(ZOrderClient zOrderRefClient)
        {
          if (zOrderRefClient._windowHandle == (IntPtr)0)
            return -1;
          if (zOrderRefClient._urlType != 3) //3 = UrlType_Video
            return -1;
          ZOrderUtils.SetParent(zOrderRefClient._parentParent.Handle, zOrderRefClient._parentParentParent.Handle);

          ZOrderUtils.SetWindowPos(zOrderRefClient._parentParent.Handle, //zOrderRefClient._parentWindowHandle,
                                    zOrderRefClient._parentParentParent.Handle,                                 //zOrderRefClient._parentParentParent.Handle, //zOrderRefClient._parentWindowHandle, 
                                    zOrderRefClient._left,
                                    zOrderRefClient._top,
                                    zOrderRefClient._right - zOrderRefClient._left, //width, 
                                    zOrderRefClient._bottom - zOrderRefClient._top, //height,
                                    (uint)0x0004 | (uint)0x4000);//| (uint)0x0001 | (uint)0x0002);            
          return 0;
        }

        //-----------------------------------
        private int putSWFClientZoneOnScreen(ZOrderClient zOrderRefClient)
        {
          if (zOrderRefClient._windowHandle == (IntPtr)0)
            return -1;
          if (zOrderRefClient._urlType != 2) //2 = UrlType_SWF
            return -1;
          ZOrderUtils.SetParent(zOrderRefClient._parentParent.Handle, zOrderRefClient._parentParentParent.Handle);

          ZOrderUtils.SetWindowPos(zOrderRefClient._parentParent.Handle, //zOrderRefClient._parentWindowHandle,
                                    zOrderRefClient._parentParentParent.Handle,                                 //zOrderRefClient._parentParentParent.Handle, //zOrderRefClient._parentWindowHandle, 
                                    zOrderRefClient._left,
                                    zOrderRefClient._top,
                                    zOrderRefClient._right - zOrderRefClient._left, //width, 
                                    zOrderRefClient._bottom - zOrderRefClient._top, //height,
                                    (uint)0x0004 | (uint)0x4000);//| (uint)0x0001 | (uint)0x0002);       SWP_ASYNCWINDOWPOS     
          return 0;
        }

        //-----------------------------------
        private int putImageClientZoneOnScreen(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;
            if (zOrderRefClient._urlType != 4) //4 = UrlType_Image
                return -1;

            ZOrderUtils.SetParent(zOrderRefClient._parentParent.Handle, zOrderRefClient._parentParentParent.Handle);

            ZOrderUtils.SetWindowPos(zOrderRefClient._parentParent.Handle, //zOrderRefClient._parentWindowHandle,
                                      zOrderRefClient._parentParentParent.Handle,                                 //zOrderRefClient._parentParentParent.Handle, //zOrderRefClient._parentWindowHandle, 
                                      zOrderRefClient._left,
                                      zOrderRefClient._top,
                                      zOrderRefClient._right - zOrderRefClient._left, //width, 
                                      zOrderRefClient._bottom - zOrderRefClient._top, //height,
                                      (uint)0x0004 | (uint)0x4000);//| (uint)0x0001 | (uint)0x0002);            

            return 0;
        }


        //-----------------------------------
        // used only for test
        private int putChromiumClientZoneOnMinusUnu(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;
            if (zOrderRefClient._urlType != 5) //5 = UrlType_HtmlChromium
                return -1;

            //zOrderRefClient._parent.Visible = false;
            //zOrderRefClient._parentParent.Visible = false;

            ZOrderUtils.SetParent(zOrderRefClient._windowHandle, zOrderRefClient._parentParentParent.Handle); //zOrderRefClient._parentWindowHandle);
            ZOrderUtils.SetWindowPos(zOrderRefClient._windowHandle,
                                      (IntPtr)(-1), //zOrderRefClient._parentParentParent.Handle, //zOrderRefClient._parentWindowHandle, 
                                      0,0, //zOrderRefClient._left, zOrderRefClient._top,
                                      zOrderRefClient._right - zOrderRefClient._left, //width, 
                                      zOrderRefClient._bottom - zOrderRefClient._top, //height,
                                      (uint)0x0001);// | (uint)0x4000);

            return 0;
        }


        //-----------------------------------
        private int putChromiumClientZoneOnScreen(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;
            if (zOrderRefClient._urlType != 5) //5 = UrlType_HtmlChromium
                return -1;
            //return putChromiumClientZoneOnMinusUnu(zOrderRefClient);

            zOrderRefClient._parent.Visible = false;
            zOrderRefClient._parentParent.Visible = false;

            int style = (int)ZOrderUtils.GetWindowLongPtr(zOrderRefClient._windowHandle, ZOrderUtils.GWL_STYLE);//GWL.STYLE);
            style = style & ~(ZOrderUtils.WS_CAPTION) & ~(ZOrderUtils.WS_THICKFRAME);// &~(ZOrderUtils.WS_POPUP); // Removes Caption bar and the sizing border
            style |= (ZOrderUtils.WS_CHILD); // Must be a child window to be hosted
            //style |= (NativeMethods.WS_CLIPCHILDREN);
            //style |= (NativeMethods.WS_CLIPSIBLINGS);
            ZOrderUtils.SetWindowLongPtr(zOrderRefClient._windowHandle, ZOrderUtils.GWL_STYLE, (IntPtr)style);
            

            ZOrderUtils.SetParent(zOrderRefClient._windowHandle, zOrderRefClient._parentParentParent.Handle); // zOrderRefClient._parentWindowHandle);//this.TopLevelControl.Handle);
            
            ZOrderUtils.SetWindowPos( zOrderRefClient._windowHandle, 
                                      zOrderRefClient._parentParentParent.Handle, //zOrderRefClient._parentWindowHandle, 
                                      zOrderRefClient._left, zOrderRefClient._top,
                                      zOrderRefClient._right - zOrderRefClient._left, //width, 
                                      zOrderRefClient._bottom - zOrderRefClient._top, //height,
                                      (uint)0x0004 | (uint)0x4000);
         
            return 0;
        }


        //===========================================================
        //  PANELS SCENARIO
        //===========================================================

        //-----------------------------------
        private int putClientZoneOnPanel(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;
            if (zOrderRefClient._urlType == 5) //5 = UrlType_HtmlChromiumWPF
              return putChromiumClientZoneOnPanel(zOrderRefClient);
            if (zOrderRefClient._urlType == 4) //4 = UrlType_Image
              return putImageClientZoneOnPanel(zOrderRefClient);
            if (zOrderRefClient._urlType == 3) //3 = UrlType_Video
              return putVideoClientZoneOnPanel(zOrderRefClient);
            if (zOrderRefClient._urlType == 2) //2 = UrlType_SWF
              return putSWFClientZoneOnPanel(zOrderRefClient);
            if (zOrderRefClient._urlType == 6) //6 = UrlType_FillColorRect
              return putFillColorRectClientZoneOnPanel(zOrderRefClient);
            if (zOrderRefClient._urlType == 7) //7 = UrlType_NoWPFChromium
                return putNonWPFChromiumClientZoneOnPanel(zOrderRefClient);
            return -1;
        }


        //-----------------------------------------------------------------
        private int putNonWPFChromiumClientZoneOnPanel(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;

            if (zOrderRefClient._urlType != 7) //7 = UrlType_NoWPFChromium
                return -1;

            return 0;
        }

        //-----------------------------------------------------------------
        private int putFillColorRectClientZoneOnPanel(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;

            if (zOrderRefClient._urlType != 6) //6 = UrlType_SWF
                return -1;

            return 0;
        }

        //-----------------------------------------------------------------
        private int putSWFClientZoneOnPanel(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;

            if (zOrderRefClient._urlType != 2) //2 = UrlType_SWF
                return -1;

            return 0;
        }

        //-----------------------------------------------------------------
        private int putVideoClientZoneOnPanel(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;

            if (zOrderRefClient._urlType != 3) //3 = UrlType_Video
                return -1;

            return 0;
        }


        //-----------------------------------------------------------------
        private int putImageClientZoneOnPanel(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;

            if (zOrderRefClient._urlType != 4) //4 = UrlType_Image
                return -1;
            //zOrderRefClient._parent.Parent.BringToFront();

            return 0;
        }


        //------------------------------------------------------------------------
        private int putChromiumClientZoneOnPanel(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;

            if (zOrderRefClient._urlType != 5) //5 = UrlType_HtmlChromium
                return -1;

            //zOrderRefClient._parent.Visible = false;
            //zOrderRefClient._parentParent.Visible = false;
            //return putClientZoneOnScreenAsTopMost(zOrderRefClient);//test

            int style = (int)ZOrderUtils.GetWindowLongPtr(zOrderRefClient._windowHandle, ZOrderUtils.GWL_STYLE);//GWL.STYLE);
            style = style & ~(ZOrderUtils.WS_CAPTION) & ~(ZOrderUtils.WS_THICKFRAME); // &~(ZOrderUtils.WS_POPUP); // Removes Caption bar and the sizing border
            style |= (ZOrderUtils.WS_CHILD); // Must be a child window to be hosted
            //style |= (NativeMethods.WS_CLIPCHILDREN);
            //style |= (NativeMethods.WS_CLIPSIBLINGS);
            //ZOrderUtils.SetWindowLong(zOrderRefClient._windowHandle, ZOrderUtils.GWL_STYLE, style);
            ZOrderUtils.SetWindowLongPtr(zOrderRefClient._windowHandle, ZOrderUtils.GWL_STYLE, (IntPtr)style);

            
            ZOrderUtils.SetParent(zOrderRefClient._windowHandle, zOrderRefClient._parentWindowHandle);//this.TopLevelControl.Handle);
            ZOrderUtils.SetWindowPos(zOrderRefClient._windowHandle, zOrderRefClient._parentWindowHandle, //zOrderRefClient._parent.Parent.Parent.Handle,
                          0, 0, // zOrderRefClient._left, zOrderRefClient._top,
                          zOrderRefClient._right - zOrderRefClient._left, //width, 
                          zOrderRefClient._bottom - zOrderRefClient._top, //height,
                          (uint)0x0004 | (uint)0x4000);
            
            return 0;
        }

        //-----------------------------------------------------
        // not-used
        private int restoreRenderingContainerZorder(ZOrderClient zOrderRefClient)
        {
            //----------------------
            if (version == 0)
            {

                RenderingContainer rc = zOrderRefClient._parent.Parent as RenderingContainer;
                //PictureBox pb = this.Parent.Parent as PictureBox;

                int nCount = zOrderRefClient._parent.Parent.Parent.FindForm().Controls.Count;
                int nNewPos = nCount - rc._zOrder - 1;
                if (nNewPos < 0)
                    nNewPos = 0;
                if (nNewPos >= nCount)
                    nNewPos = nCount - 1;
                zOrderRefClient._parent.Parent.Parent.FindForm().Controls.SetChildIndex(zOrderRefClient._parent.Parent, nNewPos);
                //zOrderRefClient._parent.Visible = false;
                //zOrderRefClient._parent.Parent.Visible = false;
            }
            //----------------------
            return 0;
        }


        //-----------------------------------
        // used only for tests
        private int putClientZoneOnScreenAsTopMost(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;
            ZOrderUtils.SetParent(zOrderRefClient._windowHandle, (IntPtr)(0));
            //ZOrderUtils.SetParent(zOrderRefClient._windowHandle, zOrderRefClient._parentWindowHandle);
            ZOrderUtils.SetWindowPos(
                zOrderRefClient._windowHandle,
                //zOrderRefClient._parentWindowHandle,
                (IntPtr)(-1),//_mainWindowHandle, //(IntPtr)(-1), //parent
                zOrderRefClient._left, zOrderRefClient._top,
                zOrderRefClient._right - zOrderRefClient._left, //width, 
                zOrderRefClient._bottom - zOrderRefClient._top, //height,
                (uint)0x0040 );//flags //to review the flags
            //ZOrderUtils.SetParent(zOrderRefClient._windowHandle, zOrderRefClient._parentWindowHandle);
            return 0;
        }

        //-----------------------------------
        // used only for tests
        private int putClientZoneOnTop(ZOrderClient zOrderRefClient)
        {
            if (zOrderRefClient._windowHandle == (IntPtr)0)
                return -1;
            ZOrderUtils.SetWindowPos(
                zOrderRefClient._windowHandle,
                (IntPtr)(-1), //zOrderRefClient._parentWindowHandle,//  _mainWindowHandle, //(IntPtr)(-1), //parent
                0, 0,
                0, //width, 
                0, //height,
                (uint)0x0040 | (uint)0x0001 | (uint)0x0002);// | (uint)0x0004);//flags //to review the flags
            return 0;
        }



        //----------------------------------------------
        private int removeZOrderClientFromASpecificList(ZOrderClient zOrderClient, List<ZOrderClient> zOrderSpecificList)
        {
            if (zOrderClient == null)
              return 1;
            if (zOrderSpecificList == null)
              return 1;
            zOrderSpecificList.Remove(zOrderClient);
            return 0;
        }

        //---------------------- 
        private List<ZOrderClient> getClientsAboveInZOrderById(int id)
        {
            ZOrderClient refClientZone = getClientById(id);
            if (refClientZone == null)
                return null;
            return getClientsAboveInZOrder(refClientZone);
        }

        //---------------------- 
        private List<ZOrderClient> getClientsAboveInZOrderByHWnd(IntPtr hWnd)
        {
            ZOrderClient refClientZone = getClientByHWnd(hWnd);
            if (refClientZone == null)
                return null;
            return getClientsAboveInZOrder(refClientZone);
        }

        //---------------------- 
        private List<ZOrderClient> getClientsAboveInZOrderByGuid(Guid guid)
        {
            ZOrderClient refClientZone = getClientByGuid(guid);
            if (refClientZone == null)
                return null;
            return getClientsAboveInZOrder(refClientZone);
        }

        //---------------------- 
        private List<ZOrderClient> getClientsAboveInZOrder(ZOrderClient zOrderRefClient)
        {
            List<ZOrderClient> zOrderAllClientsAboveList = new List<ZOrderClient>();
            List<ZOrderClient> zOrderFirstLevelClientsAboveList = null;
            foreach (ZOrderClient zOrderOtherClient in _zOrderList)
            {
                if (zOrderRefClient._id == zOrderOtherClient._id)
                    continue;

                if (isOtherClientAboveAndIsIntersected(zOrderRefClient, zOrderOtherClient))
                {
                    addClientInZOrderInAboveList(zOrderOtherClient, zOrderAllClientsAboveList);
                    if (zOrderFirstLevelClientsAboveList == null)
                        zOrderFirstLevelClientsAboveList = new List<ZOrderClient>();
                    addClientInZOrderInAboveList(zOrderOtherClient, zOrderFirstLevelClientsAboveList);
                }
            }
            if (zOrderFirstLevelClientsAboveList != null)
                addRecursivelyTheAboveClientsInZOrder(zOrderFirstLevelClientsAboveList, zOrderAllClientsAboveList);
            return zOrderAllClientsAboveList;
        }

        //---------------------- 
        private List<ZOrderClient> getClientsBelowInZOrder(ZOrderClient zOrderRefClient)
        {
            List<ZOrderClient> zOrderAllClientsBelowList = new List<ZOrderClient>();
            List<ZOrderClient> zOrderFirstLevelClientsBelowList = null;
            foreach (ZOrderClient zOrderOtherClient in _zOrderList)
            {
                if (zOrderRefClient._id == zOrderOtherClient._id)
                    continue;

                if (isOtherClientBelowAndIsIntersected(zOrderRefClient, zOrderOtherClient))
                {
                    addClientInZOrderInBelowList(zOrderOtherClient, zOrderAllClientsBelowList);
                    if (zOrderFirstLevelClientsBelowList == null)
                        zOrderFirstLevelClientsBelowList = new List<ZOrderClient>();
                    addClientInZOrderInBelowList(zOrderOtherClient, zOrderFirstLevelClientsBelowList);
                }
            }
            if (zOrderFirstLevelClientsBelowList != null)
                addRecursivelyTheBelowClientsInZOrder(zOrderFirstLevelClientsBelowList, zOrderAllClientsBelowList);
            return zOrderAllClientsBelowList;
        }


        //--------------------
        private int addRecursivelyTheAboveClientsInZOrder(List<ZOrderClient> zOrderCrtLevelClientsAboveList,
                                                          List<ZOrderClient> zOrderAllClientsAboveList)
        {
            List<ZOrderClient> zOrderNextLeveClientsAboveList = null;//new List<ZOrderClient>(); //List with the new level of above clients
            foreach (ZOrderClient zOrderExistingAboveClient in zOrderCrtLevelClientsAboveList)
            {
                //------- construct the next level above based on the crt level above 
                foreach (ZOrderClient zOrderOtherClient in _zOrderList)
                {
                    if (zOrderExistingAboveClient._id == zOrderOtherClient._id)
                        continue;

                    if (isOtherClientAboveAndIsIntersected(zOrderExistingAboveClient, zOrderOtherClient))
                    {
                        //--- if the client is already in ¨all intersected above¨ list  just continue
                        if (isClientInList(zOrderOtherClient, zOrderAllClientsAboveList))
                        {                            
                            continue;
                            //removeZOrderClientFromASpecificList(zOrderOtherClient, zOrderAllClientsAboveList);
                        }
                        //--- if the next level ¨above intersected¨ list is empty create the list
                        if (zOrderNextLeveClientsAboveList == null)
                            zOrderNextLeveClientsAboveList = new List<ZOrderClient>();

                        //-- add client in ¨next level list"
                        addClientInZOrderInAboveList(zOrderOtherClient, zOrderNextLeveClientsAboveList);

                        //-- add client in ¨all list"
                        addClientInZOrderInAboveList(zOrderOtherClient, zOrderAllClientsAboveList);
                    }
                }
            }
            //---- construct recursively the next level 
            if (zOrderNextLeveClientsAboveList != null)
                addRecursivelyTheAboveClientsInZOrder(zOrderNextLeveClientsAboveList,
                                                      zOrderAllClientsAboveList);
            return 0;
        }



        //--------------------
        private int addRecursivelyTheBelowClientsInZOrder(List<ZOrderClient> zOrderCrtLevelClientsBelowList,
                                                          List<ZOrderClient> zOrderAllClientsBelowList)
        {
            List<ZOrderClient> zOrderNextLeveClientsBelowList = null;//new List<ZOrderClient>(); //List with the new level of below clients
            foreach (ZOrderClient zOrderExistingBelowClient in zOrderCrtLevelClientsBelowList)
            {
                //------- construct the next level below based on the crt level below 
                foreach (ZOrderClient zOrderOtherClient in _zOrderList)
                {
                    if (zOrderExistingBelowClient._id == zOrderOtherClient._id)
                        continue;

                    if (isOtherClientBelowAndIsIntersected(zOrderExistingBelowClient, zOrderOtherClient))
                    {
                        //--- if the client is already in ¨all intersected above¨ list  just continue
                        if (isClientInList(zOrderOtherClient, zOrderAllClientsBelowList))
                        {
                            continue;
                            //removeZOrderClientFromASpecificList(zOrderOtherClient, zOrderAllClientsAboveList);
                        }
                        //--- if the next level ¨above intersected¨ list is empty create the list
                        if (zOrderNextLeveClientsBelowList == null)
                            zOrderNextLeveClientsBelowList = new List<ZOrderClient>();

                        //-- add client in ¨next level list"
                        addClientInZOrderInBelowList(zOrderOtherClient, zOrderNextLeveClientsBelowList);

                        //-- add client in ¨all list"
                        addClientInZOrderInBelowList(zOrderOtherClient, zOrderAllClientsBelowList);
                    }
                }
            }
            //---- construct recursively the next level 
            if (zOrderNextLeveClientsBelowList != null)
                addRecursivelyTheBelowClientsInZOrder(zOrderNextLeveClientsBelowList,
                                                      zOrderAllClientsBelowList);
            return 0;
        }

        //--------------------
        private Boolean isClientInList(ZOrderClient clientZone, List<ZOrderClient> zOrderClientList)
        {
            Boolean bExists = zOrderClientList.Contains(clientZone);
            return bExists;
        }

        //--------------------
        private int addClientInZOrderInAboveList(ZOrderClient zOrderOtherClient, List<ZOrderClient> zOrderClientAboveList)
        {
            int insertIdx = -1;
            foreach (ZOrderClient zOrderExistingAboveClient in zOrderClientAboveList)
            {
                if (isOtherClientAbove(zOrderOtherClient, zOrderExistingAboveClient))
                    insertIdx = zOrderClientAboveList.IndexOf(zOrderExistingAboveClient);
            }
            if (insertIdx != -1)
                zOrderClientAboveList.Insert(insertIdx, zOrderOtherClient);
            else
                zOrderClientAboveList.Add(zOrderOtherClient);
            return 0;
        }

        //--------------------
        private int addClientInZOrderInBelowList(ZOrderClient zOrderOtherClient, List<ZOrderClient> zOrderClientBelowList)
        {
            int insertIdx = -1;
            foreach (ZOrderClient zOrderExistingAboveClient in zOrderClientBelowList)
            {
                if (isOtherClientBelow(zOrderOtherClient, zOrderExistingAboveClient))
                    insertIdx = zOrderClientBelowList.IndexOf(zOrderExistingAboveClient);
            }
            if (insertIdx != -1)
                zOrderClientBelowList.Insert(insertIdx, zOrderOtherClient);
            else
                zOrderClientBelowList.Add(zOrderOtherClient);
            return 0;
        }

        //---------------------------
        private Boolean isOtherClientAbove(ZOrderClient zOrderRefClient, ZOrderClient zOrderOtherClient)
        {
            if (zOrderRefClient._iZOrder > zOrderOtherClient._iZOrder)
                return false;
            if (zOrderRefClient._iZOrder == zOrderOtherClient._iZOrder)
            {
                if (zOrderRefClient._iApparitionOrder > zOrderOtherClient._iApparitionOrder)
                    return false;
                if (zOrderRefClient._iZOrder == 0)
                  return false;
            }
            return true;
        }

        //---------------------------
        private Boolean isOtherClientBelow(ZOrderClient zOrderRefClient, ZOrderClient zOrderOtherClient)
        {
            if (zOrderRefClient._iZOrder < zOrderOtherClient._iZOrder)
                return false;
            if (zOrderRefClient._iZOrder == zOrderOtherClient._iZOrder)
            {
                if (zOrderRefClient._iApparitionOrder > zOrderOtherClient._iApparitionOrder)
                    return false;
                if (zOrderRefClient._iZOrder == 0)
                    return false;
            }
            return true;
        }

        //---------------------------
        private Boolean isOtherClientAboveAndIsIntersected(ZOrderClient zOrderRefClient, ZOrderClient zOrderOtherClient)
        {
            if (!isOtherClientAbove(zOrderRefClient, zOrderOtherClient))
                return false;

            //intersectRect = ! (r2.left > r1.right || r2.right < r1.left || r2.top > r1.bottom || r2.bottom < r1.top);
            Boolean bIntersect = !
                    ((zOrderOtherClient._left > zOrderRefClient._right) ||
                      (zOrderOtherClient._right < zOrderRefClient._left) ||
                      (zOrderOtherClient._top > zOrderRefClient._bottom) ||
                      (zOrderOtherClient._bottom < zOrderRefClient._top)
                    );
            return bIntersect;
        }

        //---------------------------
        private Boolean isOtherClientBelowAndIsIntersected(ZOrderClient zOrderRefClient, ZOrderClient zOrderOtherClient)
        {
            if (!isOtherClientBelow(zOrderRefClient, zOrderOtherClient))
                return false;

            //intersectRect = ! (r2.left > r1.right || r2.right < r1.left || r2.top > r1.bottom || r2.bottom < r1.top);
            Boolean bIntersect = !
                    ((zOrderOtherClient._left > zOrderRefClient._right) ||
                      (zOrderOtherClient._right < zOrderRefClient._left) ||
                      (zOrderOtherClient._top > zOrderRefClient._bottom) ||
                      (zOrderOtherClient._bottom < zOrderRefClient._top)
                    );
            return bIntersect;
        }

        //---------------------------
        private Boolean isIntersectedWithOtherClient(ZOrderClient zOrderRefClient, ZOrderClient zOrderOtherClient)
        {
            //intersectRect = ! (r2.left > r1.right || r2.right < r1.left || r2.top > r1.bottom || r2.bottom < r1.top);
            Boolean bIntersect = !
                    ((zOrderOtherClient._left > zOrderRefClient._right) ||
                      (zOrderOtherClient._right < zOrderRefClient._left) ||
                      (zOrderOtherClient._top > zOrderRefClient._bottom) ||
                      (zOrderOtherClient._bottom < zOrderRefClient._top)
                    );
            /* test
            if (zOrderRefClient._iZOrder == 1)
            {
               if (bIntersect)
               {
                    bIntersect = bIntersect;
               }
            }*/
            return bIntersect;
        }

        #endregion 

        #region shutdown methods 

        //------------------------------
        public int shutdown()
        {
          this._isShuttingDown = true ;

          //----------
          foreach (ZOrderClient zOrderClient in this._zOrderList)
          {
          }

          return 0;
        }

        //-----------------------------------
        public bool isShuttingDown()
        {
          return _isShuttingDown;
        }

        #endregion

    }
}
