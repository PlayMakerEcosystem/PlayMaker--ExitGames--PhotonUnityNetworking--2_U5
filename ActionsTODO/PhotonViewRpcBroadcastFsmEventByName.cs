// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Remote Event Calls (using Photon RPC under the hood) let you broadcast a Fsm Event as string to photon targets ( all players, other players, master).")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W920")]
	public class PhotonViewRpcBroadcastFsmEventByName : FsmStateAction
	{
		public RpcTarget rpcTarget;
		
		[Tooltip("Override photonTargets above if set. Available enums: All,AllBuffered,MasterClient,Others,OthersBuffered")]
		public FsmString photonTargetsFromString;
		
		// ONLY ACCEPTS BROADCAST OR SELF
		public FsmEventTarget eventTarget;
		
		[RequiredField]
		[Tooltip("The event name you want to send. WARNING, Make sure the event is global and exists in the first place ")]
		public FsmString remoteEvent;
		
		[Tooltip("Optionnal string data ( will be injected in the Event data. Use 'get Event Info' action to retrieve it)")]
		public FsmString stringData;
		
		
		
		
		
		public override void Reset()
		{
			// JFF: how can I set this silently without a plubic variable? if I set it to private, it doesn't work anymore. maybe I forgot a setting?
			eventTarget = new FsmEventTarget();
			eventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;
			
			remoteEvent = null;
			rpcTarget = RpcTarget.All;
			photonTargetsFromString = null;
			stringData = null;
		}

		public override void OnEnter()
		{
			DoREC();
			
			Finish();
		}

		void DoREC()
		{
			
			// get the photon proxy for Photon RPC access
			GameObject go = GameObject.Find("PlayMaker Photon Proxy");
			
			if (go == null )
			{
				return;
			}
			
			
			if (string.IsNullOrEmpty(remoteEvent.Value))
			{ 
				return;
			}
			
			RpcTarget _photonTargets = GetPhotonTargets();
		
			// get the proxy component
			PlayMakerPhotonProxy _proxy = go.GetComponent<PlayMakerPhotonProxy>();
			if (_proxy==null)
			{
				Debug.LogWarning("PlayMakerPhotonProxy is missing");
				return;
			}
			
			if (eventTarget.target == FsmEventTarget.EventTarget.BroadcastAll)
			{
				
				if (! stringData.IsNone && stringData.Value != ""){
					_proxy.PhotonRpcBroacastFsmEventWithString(_photonTargets,remoteEvent.Value,stringData.Value);
				}else{
					_proxy.PhotonRpcBroacastFsmEvent(rpcTarget,remoteEvent.Value);
				}
			}else{
				
				PlayMakerPhotonGameObjectProxy _goProxy = Owner.GetComponent<PlayMakerPhotonGameObjectProxy>();
				if (_proxy==null)
				{
						Debug.LogWarning("PlayMakerPhotonProxy is missing");
					return;
				}
				
				if (! stringData.IsNone && stringData.Value != ""){
					_goProxy.PhotonRpcSendFsmEventWithString(_photonTargets,remoteEvent.Value,stringData.Value);
				}else{
					_goProxy.PhotonRpcSendFsmEvent(rpcTarget,remoteEvent.Value);
				}
			}
			
		}
		
		RpcTarget GetPhotonTargets()
		{
			if ( photonTargetsFromString.IsNone || photonTargetsFromString.Value == "")
			{
				return rpcTarget;
			} 
			string _target = photonTargetsFromString.Value.ToLower();
			
			switch (_target)
			{
				case "all":
					return RpcTarget.All;
				
				
				case "allbuffered":
					return RpcTarget.AllBuffered;
			
				
				case "masterclient":
					return RpcTarget.MasterClient;
				
				
				case "others":
					return RpcTarget.Others;
				
				
				case "othersbuffered":
					return RpcTarget.OthersBuffered;
				
				
			}
			
			return rpcTarget;
		}
		
		public override string ErrorCheck()
		{
			
			if ( photonTargetsFromString.Value == "")
			{
				return "";
				//return "Photon target string must be set if selected.\n Available enums: All,AllBuffered,MasterClient,Others,OthersBuffered";
			} 
			
			string _target = photonTargetsFromString.Value.ToLower();
			
			switch (_target)
			{
				case "all":
					return "";
				
				
				case "allbuffered":
					return "";
				
				
				case "masterclient":
					return "";
				
				
				case "others":
					return "";
			
				
				case "othersbuffered":
					return "";
				
				
			}
			return "Photon target string must be set if selected.\n Available enums: All,AllBuffered,MasterClient,Others,OthersBuffered";
		}
		
		
		
	}
}