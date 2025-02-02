using UnityEngine;
using KBEngine;
using System; 
using System.Collections;

public class ui : MonoBehaviour {

	public static string currstate = "";
	public static bool started = false;
	public static UIAtlas uiatlas = null;
	
	void Awake ()     
	{
		if(started == false)
		{
			started = true;
			InvokeRepeating("installEvents", 0.5f, 0.0f);
		}
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDestroy()
	{
		KBEngine.Event.deregisterOut(this);
		started = false;
	}
	
	void installEvents()
	{
		CancelInvoke("installEvents");
		KBEngine.Event.registerOut("onCreateAccountResult", this, "onCreateAccountResult");
		KBEngine.Event.registerOut("onLoginFailed", this, "onLoginFailed");
		KBEngine.Event.registerOut("onVersionNotMatch", this, "onVersionNotMatch");
		KBEngine.Event.registerOut("onScriptVersionNotMatch", this, "onScriptVersionNotMatch");
		KBEngine.Event.registerOut("onLoginBaseappFailed", this, "onLoginBaseappFailed");
		KBEngine.Event.registerOut("onLoginSuccessfully", this, "onLoginSuccessfully");
		KBEngine.Event.registerOut("onLoginBaseapp", this, "onLoginBaseapp");
		KBEngine.Event.registerOut("onConnectionState", this, "onConnectionState");
		KBEngine.Event.registerOut("Loginapp_importClientMessages", this, "Loginapp_importClientMessages");
		KBEngine.Event.registerOut("Baseapp_importClientMessages", this, "Baseapp_importClientMessages");
		KBEngine.Event.registerOut("Baseapp_importClientEntityDef", this, "Baseapp_importClientEntityDef");
	}
	
	void login()
	{
		Common.DEBUG_MSG("login is Click, name=" + username.input.text + ", password=" + password.input.text + "!");
		
		log_label.obj.text = "请求连接服务器...";
		log_label.obj.color = UnityEngine.Color.green;
		if(username.input.text == "" || username.input.text.Length > 30)
		{
			log_label.obj.text = "用户名或者邮箱地址不合法。";
			log_label.obj.color = UnityEngine.Color.red;
			Common.WARNING_MSG("ui::login: invalid username!");
			return;
		}
		
		if(password.input.text.Length < 6 || password.input.text.Length > 16)
		{
			log_label.obj.text = "密码不合法, 长度应该在6-16位之间。";
			log_label.obj.color = UnityEngine.Color.red;
			Common.WARNING_MSG("ui::login: invalid reg_password!");
			return;
		}
		
		KBEngine.Event.fireIn("login", username.input.text, password.input.text, System.Text.Encoding.UTF8.GetBytes("kbengine_unity_warring"));
		log_label.obj.text = "连接成功，等待处理请稍后...";
	}
	
	void back()
	{
		if(currstate == "register")
		{
			username.input.text = reg_username.input.text;
			NGUITools.SetActive(loginbtn.button.gameObject, true);
			NGUITools.SetActive(registerbtn.button.gameObject, true);
			NGUITools.SetActive(username.input.gameObject, true);
			NGUITools.SetActive(password.input.gameObject, true);
			NGUITools.SetActive(losepasswordbtn.button.gameObject, true);
			
			NGUITools.SetActive(register_commit_btn.button.gameObject, false);
			NGUITools.SetActive(reg_username.input.gameObject, false);
			NGUITools.SetActive(reg_password.input.gameObject, false);
			NGUITools.SetActive(reg_passwordok.input.gameObject, false);
			NGUITools.SetActive(repassword_label.obj.gameObject, false);
			NGUITools.SetActive(backbtn.button.gameObject, false);
			
			reg_username.input.text = "";
			reg_password.input.text = "";
			reg_passwordok.input.text = "";
			password.input.text = "";
		}
		
		currstate = "";
	}
	
	void losepassword()
	{
		if(!KBEngineApp.validEmail(username.input.text))
		{
			log_label.obj.color = UnityEngine.Color.red;
			log_label.obj.text = "请在用户名处输入合法的邮箱地址，系统将发送一份验证邮件帮助您找回密码！";
			return;
		}
		
		KBEngineApp.app.resetpassword_loginapp(true);
	}
	
	void register()
	{
		Common.DEBUG_MSG("register is Click !");
		currstate = "register";
		
		// NGUITools.SetActive(loginpanel.panel.gameObject, false);
		NGUITools.SetActive(loginbtn.button.gameObject, false);
		NGUITools.SetActive(registerbtn.button.gameObject, false);
		NGUITools.SetActive(username.input.gameObject, false);
		NGUITools.SetActive(password.input.gameObject, false);
		NGUITools.SetActive(losepasswordbtn.button.gameObject, false);
		
		NGUITools.SetActive(register_commit_btn.button.gameObject, true);
		NGUITools.SetActive(reg_username.input.gameObject, true);
		NGUITools.SetActive(reg_password.input.gameObject, true);
		NGUITools.SetActive(reg_passwordok.input.gameObject, true);
		NGUITools.SetActive(repassword_label.obj.gameObject, true);
		NGUITools.SetActive(backbtn.button.gameObject, true);
		
		backbtn.button.transform.position = losepasswordbtn.button.transform.position;
	}
	
	void reg_ok()
	{
		log_label.obj.text = "请求连接服务器...";
		log_label.obj.color = UnityEngine.Color.green;
		
		if(reg_username.input.text == "" || reg_username.input.text.Length > 30)
		{
			log_label.obj.color = UnityEngine.Color.red;
			log_label.obj.text = "用户名或者邮箱地址不合法, 最大长度限制30个字符。";
			Common.WARNING_MSG("ui::reg_ok: invalid username!");
			return;
		}
		
		if(reg_password.input.text.Length < 6 || reg_password.input.text.Length > 16)
		{
			log_label.obj.color = UnityEngine.Color.red;
			log_label.obj.text = "密码不合法, 长度限制在6~16位之间。";
			Common.WARNING_MSG("ui::reg_ok: invalid reg_password!");
			return;
		}
		
		if(reg_password.input.text != reg_passwordok.input.text)
		{
			log_label.obj.color = UnityEngine.Color.red;
			log_label.obj.text = "二次输入密码不匹配。";
			Common.WARNING_MSG("ui::reg_ok: reg_password != reg_passwordok!");
			return;
		}
		
		KBEngine.Event.fireIn("createAccount", reg_username.input.text, reg_passwordok.input.text, System.Text.Encoding.UTF8.GetBytes("kbengine_unity_warring"));
		log_label.obj.text = "连接成功，等待处理请稍后...";
	}
		
	public void onCreateAccountResult(UInt16 retcode, byte[] datas)
	{
		log_label.obj.text = "";
		log_label.obj.color = UnityEngine.Color.red;
		
		if(retcode != 0)
		{
			log_label.obj.text = "服务器返回注册错误:" + KBEngineApp.app.serverErr(retcode) + "!";
			return;
		}
		
		log_label.obj.color = UnityEngine.Color.green;
		
		if(KBEngineApp.validEmail(username.input.text))
		{
			log_label.obj.text = "注册成功, 请进入邮箱激活账号。";
		}
		else
		{
			log_label.obj.text = "注册成功, 请点击登录按钮进入游戏！";
		}
		
		back();
	}

	public void onConnectionState(bool success)
	{
		if(!success)
		{
			log_label.obj.text = "无法连接服务器。";
			log_label.obj.color = UnityEngine.Color.red;
		}
	}
	
	public void onLoginFailed(UInt16 failedcode, byte[] serverdatas)
	{
		log_label.obj.color = UnityEngine.Color.red;
		log_label.obj.text = "登陆服务器失败, 错误:" + KBEngineApp.app.serverErr(failedcode) + "!";
	}
	
	public void onVersionNotMatch(string verInfo, string serVerInfo)
	{
		log_label.obj.color = UnityEngine.Color.red;
		log_label.obj.text = "与服务端版本(" + serVerInfo + ")不匹配, 当前版本(" + verInfo + ")!";
	}
	
	public void onScriptVersionNotMatch(string verInfo, string serVerInfo)
	{
		log_label.obj.color = UnityEngine.Color.red;
		log_label.obj.text = "与服务端脚本版本(" + serVerInfo + ")不匹配, 当前版本(" + verInfo + ")!";
	}

	public void onLoginBaseappFailed(UInt16 failedcode)
	{
		log_label.obj.color = UnityEngine.Color.red;
		log_label.obj.text = "登陆网关服务器失败, 错误:" + KBEngineApp.app.serverErr(failedcode) + "!";
	}
	
	public void onLoginBaseapp()
	{
		log_label.obj.color = UnityEngine.Color.green;
		log_label.obj.text = "请求连接到网关服务器...";
	}

	public void onLoginSuccessfully(UInt64 rndUUID, Int32 eid, Account accountEntity)
	{
		log_label.obj.color = UnityEngine.Color.green;
		log_label.obj.text = "登陆成功!";
		
		loader.inst.enterScene("selavatar");
	}
	
	public void Loginapp_importClientMessages()
	{
		log_label.obj.color = UnityEngine.Color.green;
		log_label.obj.text = "请求建立登录通信协议...";
	}

	public void Baseapp_importClientMessages()
	{
		log_label.obj.color = UnityEngine.Color.green;
		log_label.obj.text = "请求建立网关通信协议...";
	}
	
	public void Baseapp_importClientEntityDef()
	{
		log_label.obj.color = UnityEngine.Color.green;
		log_label.obj.text = "请求导入脚本...";
	}
}
