using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private PlayerListingMenu _playerListingMenu;
    [SerializeField]
    private LeaveRoomMenu _leaveRoomMenu;
    public LeaveRoomMenu LeaveRoomMenu {get {return _leaveRoomMenu;}}
       private RoomsCanvases roomsCanvases;
   public void FirstInitialize(RoomsCanvases canvases){
       roomsCanvases = canvases;
       _playerListingMenu.FirstInitialize(canvases);
       _leaveRoomMenu.FirstInitialize(canvases);
   }

   public void Show(){
       gameObject.SetActive(true);
   }

   public void Hide(){
       gameObject.SetActive(false);
   }
}
