using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using ExitGames.Client.Photon;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;
using Player = VRC.Player;


namespace VRCheat
{
	
	public class Cheat : MonoBehaviour
	{
		
		[DllImport("kernel32.dll")]
		internal static extern int AllocConsole();

		
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetConsoleWindow();

		
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		
		public void OnGUI()
		{
			GUI.Label(new Rect(1f, 0f, 300f, 20f), PhotonNetwork.playerName);
			Player[] array = PlayerManager.GetAllPlayers().Where(new Func<Player, bool>(MainClass.method_0)).ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				GUI.Label(new Rect(1f, (float)(50 + i * 15), 300f, 20f), string.Format("{0}", array[i].user.displayName));
			}
		}

		
		public void Update()
		{
			APIUser selectedUser = QuickMenu.Instance.SelectedUser;
		    VRCPlayer vrcLocal = PlayerManager.GetCurrentPlayer().vrcPlayer;


            if(selectedUser != null && !Event.current.control)
			{
				VRCPlayer vrcSelected = PlayerManager.GetPlayer(selectedUser.id).vrcPlayer;

				if (Input.GetMouseButtonDown(1))
				{
					System.Console.WriteLine("Copied {0}'s avatar!", selectedUser.displayName);
					User.CurrentUser.SetCurrentAvatar(vrcSelected.GetApiAvatar());
				}

				if (Input.GetMouseButtonDown(2))
				{
					ApiAvatar apiAvatar = vrcSelected.GetApiAvatar();
					ApiAvatar apiAvatar2 = new ApiAvatar();
					Cheat.SetForegroundWindow(Cheat.GetConsoleWindow());

					System.Console.Write("Enter avatar's name: ");
				    string avatarName = System.Console.ReadLine();

				    System.Console.Write("Enter image URL: ");
				    string avatarImageURL = System.Console.ReadLine();

				    System.Console.Write("Enter avatar description: ");
				    string avatarDesc = System.Console.ReadLine();

                    apiAvatar2.Init(User.CurrentUser, avatarName, avatarImageURL, apiAvatar.assetUrl, avatarDesc, apiAvatar.tags, apiAvatar.unityPackageUrl);
                    apiAvatar2.Save(MainClass.avatarSaved, MainClass.avatarError);
				}

                if (Input.GetKeyDown(KeyCode.T))
				{
					PlayerManager.GetCurrentPlayer().transform.position = vrcSelected.transform.position;
					PlayerManager.GetCurrentPlayer().transform.rotation = vrcSelected.transform.rotation;
				}

				if (Input.GetKeyDown(KeyCode.N))
				{
					PhotonNetwork.playerName = selectedUser.displayName;

					Hashtable propertiesToSet = new Hashtable
					{
						{
							"userId",
							User.CurrentUser.id
						}
					};

					PhotonNetwork.player.SetCustomProperties(propertiesToSet, null, false);
				}


			}

			else if (Event.current.control)
			{
			    if (Input.GetKeyDown(KeyCode.N))
				{
					Cheat.SetForegroundWindow(Cheat.GetConsoleWindow());
					System.Console.Write("Enter new name: ");
					PhotonNetwork.playerName = System.Console.ReadLine();

					Hashtable propertiesToSet2 = new Hashtable
					{
						{
							"userId",
							User.CurrentUser.id
						}
					};

					PhotonNetwork.player.SetCustomProperties(propertiesToSet2, null, false);
				}

			    if (Input.GetKeyDown(KeyCode.B))
			    {
			        ApiAvatar apiAvatar = vrcLocal.GetApiAvatar();

			        System.Console.Write("Enter image URL: ");

                    apiAvatar.imageUrl = System.Console.ReadLine();

                    apiAvatar.Save(MainClass.avatarSaved, MainClass.avatarError);

			        selectedUser.SetCurrentAvatar(apiAvatar);
			    }

                if (Input.GetKeyDown(KeyCode.G))
				{
					Cheat.Class6 @class = new Cheat.Class6();
					System.Console.Clear();
					System.Console.WriteLine(string.Join(", ", PlayerManager.GetAllPlayers().Select(new Func<Player, string>(Cheat.MainClass.getDisplayName)).ToArray<string>()));
					Cheat.SetForegroundWindow(Cheat.GetConsoleWindow());
					System.Console.Write("Teleport to: ");
					@class.string_0 = System.Console.ReadLine().ToLower();
					if (@class.string_0 != string.Empty)
					{
						Player player = PlayerManager.GetAllPlayers().FirstOrDefault(new Func<Player, bool>(@class.method_0));
						if (player != null)
						{
							System.Console.WriteLine("Teleporting to {0}", player.user.displayName);
							PlayerManager.GetCurrentPlayer().transform.position = player.transform.position;
							PlayerManager.GetCurrentPlayer().transform.rotation = player.transform.rotation;
						}
					}
				}

				if (Input.GetKeyDown(KeyCode.Delete))
				{
					System.Console.WriteLine("Avatar(\"{0}\", {1}) deleted!", User.CurrentUser.apiAvatar.name, User.CurrentUser.apiAvatar.id);
					ApiAvatar.Delete(User.CurrentUser.apiAvatar.id, new Action(Cheat.MainClass.avatarDel), new Action<string>(Cheat.MainClass.avatarDelError));
				}

				if (Input.GetKeyDown(KeyCode.M))
				{
					new Thread(new ThreadStart(Cheat.MainClass.pickupItemsToMe)).Start();
				}

				if (Input.GetKeyDown(KeyCode.B))
				{
				    ApiAvatar avatar = vrcLocal.GetApiAvatar();

				    System.Console.WriteLine(avatar.name + ":" + avatar.assetUrl + ":" + avatar.assetVersion + ":" + avatar.authorId + ":" + avatar.authorName + ":" + avatar.id + ":" + avatar.imageUrl);
                    
                }

                if (Input.GetMouseButtonDown(2))
				{
					System.Console.Write("Enter avatar ID: ");
					Cheat.SetForegroundWindow(Cheat.GetConsoleWindow());
					ApiAvatar.Fetch(System.Console.ReadLine(), Cheat.MainClass.saveAvatar, Cheat.MainClass.method_16);
				}

				if (Input.GetKeyDown(KeyCode.O))
                {
					Cheat.FollowUser followUser = new Cheat.FollowUser();
					Cheat.SetForegroundWindow(Cheat.GetConsoleWindow());
					System.Console.Write("Follow: ");
					followUser.UserName = System.Console.ReadLine();
					APIUser.FetchUsers(followUser.UserName, followUser.onSuccess,Cheat.MainClass.fetchUserErr);
				}

		        if (Input.GetKeyDown(KeyCode.F))
		        {
		            this.speedhack = !this.speedhack;
		            if (this.speedhack)
		            {
		                this.vector3_0 = Physics.gravity;
		                Physics.gravity = Vector3.zero;
		            }
		            else
		            {
		                Physics.gravity = this.vector3_0;
		            }
		        }

		        if (Input.GetKeyDown(KeyCode.R))
		        {
		            this.flyMode = !this.flyMode;
		        }
		    }

		    if (this.locomotionInputController == null)
		    {
		        this.locomotionInputController = vrcLocal.GetComponent<LocomotionInputController>();
		        this.vrcmotionState = (VRCMotionState)typeof(LocomotionInputController).GetField("motionState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this.locomotionInputController);
		    }

		    bool key = Input.GetKey(KeyCode.LeftShift);

		    if (this.speedhack)
		    {
		        this.vrcmotionState.Reset();
		        Vector3 position = vrcLocal.transform.position;

		        if (Input.GetKey(KeyCode.E))
		        {
		            vrcLocal.transform.position = new Vector3(position.x, position.y + (float)(key ? 15 : 4) * Time.deltaTime, position.z);
		        }

		        if (Input.GetKey(KeyCode.Q))
		        {
		            vrcLocal.transform.position = new Vector3(position.x, position.y - (float)(key ? 15 : 4) * Time.deltaTime, position.z);
		        }
		    }

		    this.locomotionInputController.strafeSpeed = (float)(this.speedhack ? (key ? 15 : 4) : (key ? 8 : 2));
		    this.locomotionInputController.runSpeed = (float)(this.speedhack ? 15 : 4);

            if (this.flyMode && DateTime.Now.Millisecond % 500 <= 10)
			{
				if (this.randomName)
				{
					PhotonNetwork.playerName = Path.GetRandomFileName();
					Hashtable propertiesToSet3 = new Hashtable
					{
						{
							"userId",
							User.CurrentUser.id
						}
					};
					PhotonNetwork.player.SetCustomProperties(propertiesToSet3, null, false);
					this.randomName = false;
					return;
				}
			}
			else
			{
				this.randomName = true;
			}
		}

		
		public void Start()
		{
		}

		
		public void Awake()
		{
			Cheat.AllocConsole();
			System.Console.Title = "VRCheat by www.TEAMGAMERFOOD.com";
			System.Console.SetOut(new StreamWriter(System.Console.OpenStandardOutput())
			{
				AutoFlush = true
			});
			System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput()));
			AppDomain.CurrentDomain.UnhandledException += this.method_0;
			System.Console.WriteLine("Created by A5");
			Cheat.SetForegroundWindow(Cheat.GetConsoleWindow());
			System.Console.WriteLine("VRChat {0}", VRCApplicationSetup.GetBuildVersionString());
			System.Console.Title = string.Format("VRCheat by www.TEAMGAMERFOOD.com (VRChat {0})", VRCApplicationSetup.GetBuildVersionString());
			UnityEngine.Object.DontDestroyOnLoad(this);
		}

		
		private void method_0(object sender, UnhandledExceptionEventArgs e)
		{
			System.Console.WriteLine(((Exception)e.ExceptionObject).Message);
		}

		
		static QuickMenu smethod_0()
		{
			return QuickMenu.Instance;
		}

		
		static APIUser smethod_1(QuickMenu quickMenu_0)
		{
			return quickMenu_0.SelectedUser;
		}

		
		static Player smethod_2()
		{
			return PlayerManager.GetCurrentPlayer();
		}

		
		static string smethod_3(ApiModel apiModel_0)
		{
			return apiModel_0.id;
		}

		
		static Player smethod_4(string string_0)
		{
			return PlayerManager.GetPlayer(string_0);
		}

		
		static bool smethod_5(int int_0)
		{
			return Input.GetMouseButtonDown(int_0);
		}

		
		static string smethod_6(APIUser apiuser_0)
		{
			return apiuser_0.displayName;
		}

		
		static void smethod_7(string string_0, object object_0)
		{
			System.Console.WriteLine(string_0, object_0);
		}

		
		static User smethod_8()
		{
			return User.CurrentUser;
		}

		
		static void smethod_9(APIUser apiuser_0, ApiAvatar apiAvatar_0)
		{
			apiuser_0.SetCurrentAvatar(apiAvatar_0);
		}

		
		static ApiAvatar smethod_10()
		{
			return new ApiAvatar();
		}

		
		static void smethod_11(string string_0)
		{
			System.Console.Write(string_0);
		}

		
		static string smethod_12()
		{
			return System.Console.ReadLine();
		}

		
		static string smethod_13(ApiAvatar apiAvatar_0)
		{
			return apiAvatar_0.imageUrl;
		}

		
		static string smethod_14(ApiAvatar apiAvatar_0)
		{
			return apiAvatar_0.assetUrl;
		}

		
		static string smethod_15(ApiAvatar apiAvatar_0)
		{
			return apiAvatar_0.description;
		}

		
		static List<string> smethod_16(ApiAvatar apiAvatar_0)
		{
			return apiAvatar_0.tags;
		}

		
		static string smethod_17(ApiAvatar apiAvatar_0)
		{
			return apiAvatar_0.unityPackageUrl;
		}

		
		static void smethod_18(ApiAvatar apiAvatar_0, APIUser apiuser_0, string string_0, string string_1, string string_2, string string_3, List<string> list_0, string string_4)
		{
			apiAvatar_0.Init(apiuser_0, string_0, string_1, string_2, string_3, list_0, string_4);
		}

		
		static void smethod_19(ApiAvatar apiAvatar_0, Action<ApiModel> action_0, Action<string> action_1)
		{
			apiAvatar_0.Save(action_0, action_1);
		}

		
		static bool smethod_20(KeyCode keyCode_0)
		{
			return Input.GetKeyDown(keyCode_0);
		}

		
		static Transform smethod_21(Component component_0)
		{
			return component_0.transform;
		}

		
		static Vector3 smethod_22(Transform transform_0)
		{
			return transform_0.position;
		}

		
		static void smethod_23(Transform transform_0, Vector3 vector3_1)
		{
			transform_0.position = vector3_1;
		}

		
		static Quaternion smethod_24(Transform transform_0)
		{
			return transform_0.rotation;
		}

		
		static void smethod_25(Transform transform_0, Quaternion quaternion_0)
		{
			transform_0.rotation = quaternion_0;
		}

		
		static void smethod_26(string string_0)
		{
			PhotonNetwork.playerName = string_0;
		}

		
		static Hashtable smethod_27()
		{
			return new Hashtable();
		}

		
		static PhotonPlayer smethod_28()
		{
			return PhotonNetwork.player;
		}

		
		static void smethod_29(PhotonPlayer photonPlayer_0, Hashtable hashtable_0, Hashtable hashtable_1, bool bool_3)
		{
			photonPlayer_0.SetCustomProperties(hashtable_0, hashtable_1, bool_3);
		}

		
		static void smethod_30(string string_0, ApiNotification.NotificationType notificationType_0, string string_1, Dictionary<string, object> dictionary_0, Action<ApiNotification> action_0, Action<string> action_1)
		{
			ApiNotification.SendNotification(string_0, notificationType_0, string_1, dictionary_0, action_0, action_1);
		}

		
		static bool smethod_31(KeyCode keyCode_0)
		{
			return Input.GetKey(keyCode_0);
		}

		
		static Event smethod_32()
		{
			return Event.current;
		}

		
		static bool smethod_33(Event event_0)
		{
			return event_0.control;
		}

		
		static void smethod_34()
		{
			System.Console.Clear();
		}

		
		static Player[] smethod_35()
		{
			return PlayerManager.GetAllPlayers();
		}

		
		static string smethod_36(string string_0, string[] string_1)
		{
			return string.Join(string_0, string_1);
		}

		
		static void smethod_37(string string_0)
		{
			System.Console.WriteLine(string_0);
		}

		
		static string smethod_38(string string_0)
		{
			return string_0.ToLower();
		}

		
		static bool smethod_39(string string_0, string string_1)
		{
			return string_0 != string_1;
		}

		
		static bool smethod_40(UnityEngine.Object object_0, UnityEngine.Object object_1)
		{
			return object_0 != object_1;
		}

		
		static APIUser smethod_41(Player player_0)
		{
			return player_0.user;
		}

		
		static ApiAvatar smethod_42(User user_0)
		{
			return user_0.apiAvatar;
		}

		
		static string smethod_43(ApiAvatar apiAvatar_0)
		{
			return apiAvatar_0.name;
		}

		
		static string smethod_44(ApiAvatar apiAvatar_0)
		{
			return apiAvatar_0.id;
		}

		
		static void smethod_45(string string_0, object object_0, object object_1)
		{
			System.Console.WriteLine(string_0, object_0, object_1);
		}

		
		static void smethod_46(string string_0, Action action_0, Action<string> action_1)
		{
			ApiAvatar.Delete(string_0, action_0, action_1);
		}

		
		static Thread smethod_47(ThreadStart threadStart_0)
		{
			return new Thread(threadStart_0);
		}

		
		static void smethod_48(Thread thread_0)
		{
			thread_0.Start();
		}

		
		static Vector3 smethod_49()
		{
			return Physics.gravity;
		}

		
		static void smethod_50(Vector3 vector3_1)
		{
			Physics.gravity = vector3_1;
		}

		
		static void smethod_51(string string_0, Action<List<APIUser>> action_0, Action<string> action_1)
		{
			APIUser.FetchUsers(string_0, action_0, action_1);
		}

		
		static void smethod_52(string string_0, Action<ApiAvatar> action_0, Action<string> action_1)
		{
			ApiAvatar.Fetch(string_0, action_0, action_1);
		}

		
		static bool smethod_53(UnityEngine.Object object_0, UnityEngine.Object object_1)
		{
			return object_0 == object_1;
		}

		
		static Type smethod_54(RuntimeTypeHandle runtimeTypeHandle_0)
		{
			return Type.GetTypeFromHandle(runtimeTypeHandle_0);
		}

		
		static FieldInfo smethod_55(Type type_0, string string_0, BindingFlags bindingFlags_0)
		{
			return type_0.GetField(string_0, bindingFlags_0);
		}

		
		static object smethod_56(FieldInfo fieldInfo_0, object object_0)
		{
			return fieldInfo_0.GetValue(object_0);
		}

		
		static void smethod_57(VRCMotionState vrcmotionState_1)
		{
			vrcmotionState_1.Reset();
		}

		
		static float smethod_58()
		{
			return Time.deltaTime;
		}

		
		static void smethod_59(string string_0)
		{
			System.Console.Title = string_0;
		}

		
		static Stream smethod_60()
		{
			return System.Console.OpenStandardOutput();
		}

		
		static StreamWriter smethod_61(Stream stream_0)
		{
			return new StreamWriter(stream_0);
		}

		
		static void smethod_62(StreamWriter streamWriter_0, bool bool_3)
		{
			streamWriter_0.AutoFlush = bool_3;
		}

		
		static void smethod_63(TextWriter textWriter_0)
		{
			System.Console.SetOut(textWriter_0);
		}

		
		static Stream smethod_64()
		{
			return System.Console.OpenStandardInput();
		}

		
		static StreamReader smethod_65(Stream stream_0)
		{
			return new StreamReader(stream_0);
		}

		
		static void smethod_66(TextReader textReader_0)
		{
			System.Console.SetIn(textReader_0);
		}

		
		static AppDomain smethod_67()
		{
			return AppDomain.CurrentDomain;
		}

		
		static void smethod_68(AppDomain appDomain_0, UnhandledExceptionEventHandler unhandledExceptionEventHandler_0)
		{
			appDomain_0.UnhandledException += unhandledExceptionEventHandler_0;
		}

		
		static VRCApplicationSetup smethod_69()
		{
			return VRCApplicationSetup.Instance;
		}

		
		static void smethod_70(ConsoleColor consoleColor_0)
		{
			System.Console.ForegroundColor = consoleColor_0;
		}

		
		static void smethod_71()
		{
			Application.Quit();
		}

		
		static string smethod_72()
		{
			return VRCApplicationSetup.GetBuildVersionString();
		}

		
		static string smethod_73(string string_0, object object_0)
		{
			return string.Format(string_0, object_0);
		}

		
		static void smethod_74(UnityEngine.Object object_0)
		{
			UnityEngine.Object.DontDestroyOnLoad(object_0);
		}

		
		static object smethod_75(UnhandledExceptionEventArgs unhandledExceptionEventArgs_0)
		{
			return unhandledExceptionEventArgs_0.ExceptionObject;
		}

		
		static string smethod_76(Exception exception_0)
		{
			return exception_0.Message;
		}

		
		private bool flyMode;

		
		private bool randomName;

		
		private bool speedhack;

		
		private Vector3 vector3_0;

		
		private LocomotionInputController locomotionInputController;

		
		private VRCMotionState vrcmotionState;

		public class MainClass
		{
			
			public static bool method_0(Player player_0)
			{
				return player_0.isModerator;
			}

			
			public static void avatarSaved(ApiModel apiModel_0)
			{
				System.Console.WriteLine("Avatar saved!");
			}

            
		    public static void avatarError(string string_0)
			{
				System.Console.WriteLine("Error saving avatar: {0}", string_0);
			}

            
		    public static void msgSent(ApiNotification apiNotification_0)
			{
				System.Console.WriteLine("Message sent!");
			}

            
		    public static void msgError(string string_0)
			{
				System.Console.WriteLine("Error sending message");
			}

            
		    public static string getDisplayName(Player player_0)
			{
				return player_0.user.displayName;
			}

            
		    public static void avatarDel()
			{
				System.Console.WriteLine("Avatar deleted!");
			}

            
		    public static void avatarDelError(string string_0)
			{
				System.Console.WriteLine("Error deleting avatar: {0}", string_0);
			}

            
		    public static void pickupItemsToMe()
			{
				ObjectInternal[] array = UnityEngine.Object.FindObjectsOfType<ObjectInternal>();
				System.Console.WriteLine(array.Length);
				foreach (ObjectInternal objectInternal in array)
				{
					if (objectInternal.OwnerUserId != PlayerManager.GetCurrentPlayer().userId)
					{
						objectInternal.RequestOwnership();
					}
					Thread.Sleep(200);
					if (typeof(ObjectInternal).GetField("pickup", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(objectInternal) != null)
					{
						objectInternal.transform.position = PlayerManager.GetCurrentPlayer().transform.position;
					}
				}
			}

            
		    public static int getDisplayNameLen(APIUser apiuser_0)
			{
				return apiuser_0.displayName.Length;
			}

			
			public static void onError(string string_0)
			{
				System.Console.WriteLine("Error: {0}", string_0);
			}

            
		    public static void fetchWorldErr(string string_0)
			{
				System.Console.WriteLine("Error fetching world: {0}", string_0);
			}

            
		    public static void fetchUserErr(string string_0)
			{
				System.Console.WriteLine("Error fetching user: {0}", string_0);
			}

            
		    public static void saveAvatar(ApiAvatar apiAvatar_0)
			{
				System.Console.Write("Enter avatar's name: ");
				Cheat.SetForegroundWindow(Cheat.GetConsoleWindow());
				ApiAvatar apiAvatar = new ApiAvatar();
				apiAvatar.Init(User.CurrentUser, System.Console.ReadLine(), apiAvatar_0.imageUrl, apiAvatar_0.assetUrl, apiAvatar_0.description, apiAvatar_0.tags, apiAvatar_0.unityPackageUrl);
				apiAvatar.Save(new Action<ApiModel>(Cheat.MainClass.msgAvatarSaved), new Action<string>(Cheat.MainClass.method_15));
			}

			
			public static void msgAvatarSaved(ApiModel apiModel_0)
			{
				System.Console.WriteLine("Avatar saved!");
			}

            
		    public static void method_15(string string_0)
			{
				System.Console.WriteLine("Error saving avatar: {0}", string_0);
			}

            
		    public static void method_16(string string_0)
			{
				System.Console.WriteLine("Error saving avatar: {0}", string_0);
			}

			
			static bool smethod_0(Player player_0)
			{
				return player_0.isModerator;
			}

			
			static void smethod_1(string string_0)
			{
				System.Console.WriteLine(string_0);
			}

			
			static void smethod_2(string string_0, object object_0)
			{
				System.Console.WriteLine(string_0, object_0);
			}

			
			static APIUser smethod_3(Player player_0)
			{
				return player_0.user;
			}

			
			static string smethod_4(APIUser apiuser_0)
			{
				return apiuser_0.displayName;
			}

			
			static void smethod_5(int int_0)
			{
				System.Console.WriteLine(int_0);
			}

			
			static string smethod_6(VRCPunBehaviour vrcpunBehaviour_0)
			{
				return vrcpunBehaviour_0.OwnerUserId;
			}

			
			static Player smethod_7()
			{
				return PlayerManager.GetCurrentPlayer();
			}

			
			static string smethod_8(Player player_0)
			{
				return player_0.userId;
			}

			
			static bool smethod_9(string string_0, string string_1)
			{
				return string_0 != string_1;
			}

			
			static void smethod_10(VRCPunBehaviour vrcpunBehaviour_0)
			{
				vrcpunBehaviour_0.RequestOwnership();
			}

			
			static void smethod_11(int int_0)
			{
				Thread.Sleep(int_0);
			}

			
			static Type smethod_12(RuntimeTypeHandle runtimeTypeHandle_0)
			{
				return Type.GetTypeFromHandle(runtimeTypeHandle_0);
			}

			
			static FieldInfo smethod_13(Type type_0, string string_0, BindingFlags bindingFlags_0)
			{
				return type_0.GetField(string_0, bindingFlags_0);
			}

			
			static object smethod_14(FieldInfo fieldInfo_0, object object_0)
			{
				return fieldInfo_0.GetValue(object_0);
			}

			
			static Transform smethod_15(Component component_0)
			{
				return component_0.transform;
			}

			
			static Vector3 smethod_16(Transform transform_0)
			{
				return transform_0.position;
			}

			
			static void smethod_17(Transform transform_0, Vector3 vector3_0)
			{
				transform_0.position = vector3_0;
			}

			
			static int smethod_18(string string_0)
			{
				return string_0.Length;
			}

			
			static void smethod_19(string string_0)
			{
				System.Console.Write(string_0);
			}

			
			static ApiAvatar smethod_20()
			{
				return new ApiAvatar();
			}

			
			static User smethod_21()
			{
				return User.CurrentUser;
			}

			
			static string smethod_22()
			{
				return System.Console.ReadLine();
			}

			
			static string smethod_23(ApiAvatar apiAvatar_0)
			{
				return apiAvatar_0.imageUrl;
			}

			
			static string smethod_24(ApiAvatar apiAvatar_0)
			{
				return apiAvatar_0.assetUrl;
			}

			
			static string smethod_25(ApiAvatar apiAvatar_0)
			{
				return apiAvatar_0.description;
			}

			
			static List<string> smethod_26(ApiAvatar apiAvatar_0)
			{
				return apiAvatar_0.tags;
			}

			
			static string smethod_27(ApiAvatar apiAvatar_0)
			{
				return apiAvatar_0.unityPackageUrl;
			}

			
			static void smethod_28(ApiAvatar apiAvatar_0, APIUser apiuser_0, string string_0, string string_1, string string_2, string string_3, List<string> list_0, string string_4)
			{
				apiAvatar_0.Init(apiuser_0, string_0, string_1, string_2, string_3, list_0, string_4);
			}

			
			static void smethod_29(ApiAvatar apiAvatar_0, Action<ApiModel> action_0, Action<string> action_1)
			{
				apiAvatar_0.Save(action_0, action_1);
			}

			
		}

		
		[CompilerGenerated]
		private sealed class Class6
		{
			
			internal bool method_0(Player player_0)
			{
				return player_0.user.displayName.ToLower().Contains(this.string_0);
			}

			
			static APIUser smethod_0(Player player_0)
			{
				return player_0.user;
			}

			
			static string smethod_1(APIUser apiuser_0)
			{
				return apiuser_0.displayName;
			}

			
			static string smethod_2(string string_1)
			{
				return string_1.ToLower();
			}

			
			static bool smethod_3(string string_1, string string_2)
			{
				return string_1.Contains(string_2);
			}

			
			public string string_0;
		}

		
		[CompilerGenerated]
		private sealed class FollowUser
		{
			
			internal void onSuccess(List<APIUser> apiUserList)
			{
				Cheat.CApiUser cApiUser = new Cheat.CApiUser();
				Cheat.CApiUser apiUser = cApiUser;

                Func<APIUser, bool> predicate;
				if ((predicate = this.func_0) == null)
				{
					predicate = (this.func_0 = this.method_1);
				}
				apiUser.apiuser_0 = apiUserList.Where(predicate).OrderBy(Cheat.MainClass.getDisplayNameLen).FirstOrDefault<APIUser>();
				if (cApiUser.apiuser_0 == null)
				{
					System.Console.WriteLine("User not found!");
					return;
				}
				if (cApiUser.apiuser_0.location == "offline")
				{
					System.Console.WriteLine("User is offline!");
					return;
				}
				if (cApiUser.apiuser_0.location.Split(':').Length > 1)
				{
					Cheat.FollowUserHelper followUserHelper = new Cheat.FollowUserHelper();
					followUserHelper.apiUser = cApiUser;

					string id = followUserHelper.apiUser.apiuser_0.location.Split(':')[0];

					followUserHelper.string_0 = followUserHelper.apiUser.apiuser_0.location.Split(':')[1];

					ApiWorld.Fetch(id, followUserHelper.method_0, Cheat.MainClass.fetchWorldErr);
					return;
				}
				System.Console.WriteLine("Could not parse user location \"{0}\"", cApiUser.apiuser_0.location);
			}

			
			internal bool method_1(APIUser apiuser_0)
			{
				return apiuser_0.displayName.ToLower().Contains(this.UserName.ToLower());
			}

			
			static string smethod_0(APIUser apiuser_0)
			{
				return apiuser_0.location;
			}

			
			static bool smethod_1(string string_1, string string_2)
			{
				return string_1 != string_2;
			}

			
			static string[] smethod_2(string string_1, char[] char_0)
			{
				return string_1.Split(char_0);
			}

			
			static void smethod_3(string string_1, Action<ApiWorld> action_0, Action<string> action_1)
			{
				ApiWorld.Fetch(string_1, action_0, action_1);
			}

			
			static void smethod_4(string string_1, object object_0)
			{
				System.Console.WriteLine(string_1, object_0);
			}

			
			static void smethod_5(string string_1)
			{
				System.Console.WriteLine(string_1);
			}

			
			static string smethod_6(APIUser apiuser_0)
			{
				return apiuser_0.displayName;
			}

			
			static string smethod_7(string string_1)
			{
				return string_1.ToLower();
			}

			
			static bool smethod_8(string string_1, string string_2)
			{
				return string_1.Contains(string_2);
			}

			
			public string UserName;

			
			public Func<APIUser, bool> func_0;
		}

		
		[CompilerGenerated]
		private sealed class CApiUser
		{
			
			public APIUser apiuser_0;
		}

		
		[CompilerGenerated]
		private sealed class FollowUserHelper
		{
			
			internal void method_0(ApiWorld apiWorld_0)
			{
				System.Console.WriteLine("{0} is in {1} ({2})", this.apiUser.apiuser_0.displayName, apiWorld_0.name, ApiWorld.WorldInstance.GetAccessDetail(new ApiWorld.WorldInstance(this.string_0, 1).GetAccessType()).fullName);
				System.Console.Write("Follow {0}? (y/n): ", this.apiUser.apiuser_0.displayName);
				if (System.Console.ReadLine().ToLower() == "y")
				{
					System.Console.WriteLine("Following {0}", this.apiUser.apiuser_0.displayName);
					VRCFlowManager.Instance.EnterRoom(this.apiUser.apiuser_0.location, Cheat.MainClass.onError);
				}
			}

			
			static string smethod_0(APIUser apiuser_0)
			{
				return apiuser_0.displayName;
			}

			
			static string smethod_1(ApiWorld apiWorld_0)
			{
				return apiWorld_0.name;
			}

			
			static ApiWorld.WorldInstance smethod_2(string string_1, int int_0)
			{
				return new ApiWorld.WorldInstance(string_1, int_0);
			}

			
			static ApiWorld.WorldInstance.AccessType smethod_3(ApiWorld.WorldInstance worldInstance_0)
			{
				return worldInstance_0.GetAccessType();
			}

			
			static ApiWorld.WorldInstance.AccessDetail smethod_4(ApiWorld.WorldInstance.AccessType accessType_0)
			{
				return ApiWorld.WorldInstance.GetAccessDetail(accessType_0);
			}

			
			static string smethod_5(ApiWorld.WorldInstance.AccessDetail accessDetail_0)
			{
				return accessDetail_0.fullName;
			}

			
			static void smethod_6(string string_1, object object_0, object object_1, object object_2)
			{
				System.Console.WriteLine(string_1, object_0, object_1, object_2);
			}

			
			static void smethod_7(string string_1, object object_0)
			{
				System.Console.Write(string_1, object_0);
			}

			
			static string smethod_8()
			{
				return System.Console.ReadLine();
			}

			
			static string smethod_9(string string_1)
			{
				return string_1.ToLower();
			}

			
			static bool smethod_10(string string_1, string string_2)
			{
				return string_1 == string_2;
			}

			
			static void smethod_11(string string_1, object object_0)
			{
				System.Console.WriteLine(string_1, object_0);
			}

			
			static VRCFlowManager smethod_12()
			{
				return VRCFlowManager.Instance;
			}

			
			static string smethod_13(APIUser apiuser_0)
			{
				return apiuser_0.location;
			}

			
			static void smethod_14(VRCFlowManager vrcflowManager_0, string string_1, Action<string> action_0)
			{
				vrcflowManager_0.EnterRoom(string_1, action_0);
			}

			
			public string string_0;

			
			public Cheat.CApiUser apiUser;
		}
	}
}
