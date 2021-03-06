/*
 * Copyright (c) 1997-2004 Apple Computer, Inc. All rights reserved.
 *
 * @APPLE_LICENSE_HEADER_START@
 * 
 * This file contains Original Code and/or Modifications of Original Code
 * as defined in and that are subject to the Apple Public Source License
 * Version 2.0 (the 'License'). You may not use this file except in
 * compliance with the License. Please obtain a copy of the License at
 * http://www.opensource.apple.com/apsl/ and read it before using this
 * file.
 * 
 * The Original Code and all software distributed under the License are
 * distributed on an 'AS IS' basis, WITHOUT WARRANTY OF ANY KIND, EITHER
 * EXPRESS OR IMPLIED, AND APPLE HEREBY DISCLAIMS ALL SUCH WARRANTIES,
 * INCLUDING WITHOUT LIMITATION, ANY WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE, QUIET ENJOYMENT OR NON-INFRINGEMENT.
 * Please see the License for the specific language governing rights and
 * limitations under the License.
 * 
 * @APPLE_LICENSE_HEADER_END@

    Change History (most recent first):
    
$Log: DNSServiceBrowser.cs,v $
Revision 1.5  2004/09/21 16:26:58  shersche
Check to make sure browse list selected item is not null before resolving
Submitted by: prepin@gmail.com

Revision 1.4  2004/09/13 19:38:17  shersche
Changed code to reflect namespace and type changes to dnssd.NET library

Revision 1.3  2004/09/11 00:38:14  shersche
DNSService APIs now assume port in host format. Check for null text record in resolve callback.

Revision 1.2  2004/07/22 23:15:25  shersche
Fix service names for teleport, tftp, and bootps

Revision 1.1  2004/07/19 07:54:24  shersche
Initial revision



*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using Apple.DNSSD;

namespace DNSServiceBrowser_NET
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox typeBox;
		private System.Windows.Forms.ListBox browseList;
		private ServiceRef browser = null;
		private ServiceRef resolver = null;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		//
		// These delegates are invoked as a result of DNSService
		// operation.
		//
		delegate void AddServiceCallback(BrowseData data);
		delegate void RemoveServiceCallback(BrowseData data);
		delegate void ResolveServiceCallback(ResolveData data);

		AddServiceCallback		addServiceCallback;
		RemoveServiceCallback	removeServiceCallback;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox nameField;
		private System.Windows.Forms.TextBox typeField;
		private System.Windows.Forms.TextBox domainField;
		private System.Windows.Forms.TextBox hostField;
		private System.Windows.Forms.TextBox portField;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ListBox serviceTextField;
		ResolveServiceCallback	resolveServiceCallback;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			addServiceCallback		= new AddServiceCallback(OnAddService);
			removeServiceCallback	= new RemoveServiceCallback(OnRemoveService);
			resolveServiceCallback	= new ResolveServiceCallback(OnResolveService);

			this.Load += new System.EventHandler(this.Form1_Load);
		}

		private void Form1_Load(object sender, EventArgs e) 
		{
			typeBox.SelectedItem = "_spike._tcp";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}

				if (browser != null)
				{
					browser.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.browseList = new System.Windows.Forms.ListBox();
			this.typeBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.nameField = new System.Windows.Forms.TextBox();
			this.typeField = new System.Windows.Forms.TextBox();
			this.domainField = new System.Windows.Forms.TextBox();
			this.hostField = new System.Windows.Forms.TextBox();
			this.portField = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.serviceTextField = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// browseList
			// 
			this.browseList.Location = new System.Drawing.Point(8, 48);
			this.browseList.Name = "browseList";
			this.browseList.Size = new System.Drawing.Size(488, 147);
			this.browseList.TabIndex = 0;
			this.browseList.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// typeBox
			// 
			this.typeBox.Items.AddRange(new object[]
			{
				"_accessone._tcp",
				"_accountedge._tcp",
				"_actionitems._tcp",
				"_addressbook._tcp",
				"_aecoretech._tcp",
				"_afpovertcp._tcp",
				"_airport._tcp",
				"_animolmd._tcp",
				"_animobserver._tcp",
				"_apple-sasl._tcp",
				"_aquamon._tcp",
				"_async._tcp",
				"_auth._tcp",
				"_beep._tcp",
				"_bfagent._tcp",
				"_bootps._udp",
				"_bousg._tcp",
				"_bsqdea._tcp",
				"_cheat._tcp",
				"_chess._tcp",
				"_clipboard._tcp",
				"_collection._tcp",
				"_contactserver._tcp",
				"_cvspserver._tcp",
				"_cytv._tcp",
				"_daap._tcp",
				"_difi._tcp",
				"_distcc._tcp",
				"_dossier._tcp",
				"_dpap._tcp",
				"_earphoria._tcp",
				"_ebms._tcp",
				"_ebreg._tcp",
				"_ecbyesfsgksc._tcp",
				"_eheap._tcp",
				"_embrace._tcp",
				"_eppc._tcp",
				"_eventserver._tcp",
				"_exec._tcp",
				"_facespan._tcp",
				"_faxstfx._tcp",
				"_fish._tcp",
				"_fjork._tcp",
				"_fmpro-internal._tcp",
				"_ftp._tcp",
				"_ftpcroco._tcp",
				"_gbs-smp._tcp",
				"_gbs-stp._tcp",
				"_grillezvous._tcp",
				"_h323._tcp",
				"_http._tcp",
				"_hotwayd._tcp",
				"_hydra._tcp",
				"_ica-networking._tcp",
				"_ichalkboard._tcp",
				"_ichat._tcp",
				"_iconquer._tcp",
				"_ifolder._tcp",
				"_ilynx._tcp",
				"_imap._tcp",
				"_imidi._tcp",
				"_ipbroadcaster._tcp",
				"_ipp._tcp",
				"_isparx._tcp",
				"_ispq-vc._tcp",
				"_ishare._tcp",
				"_isticky._tcp",
				"_istorm._tcp",
				"_iwork._tcp",
				"_lan2p._tcp",
				"_ldap._tcp",
				"_liaison._tcp",
				"_login._tcp",
				"_lontalk._tcp",
				"_lonworks._tcp",
				"_macfoh-remote._tcp",
				"_macminder._tcp",
				"_moneyworks._tcp",
				"_mp3sushi._tcp",
				"_mttp._tcp",
				"_ncbroadcast._tcp",
				"_ncdirect._tcp",
				"_ncsyncserver._tcp",
				"_net-assistant._tcp",
				"_newton-dock._tcp",
				"_nfs._udp",
				"_nssocketport._tcp",
				"_odabsharing._tcp",
				"_omni-bookmark._tcp",
				"_openbase._tcp",
				"_p2pchat._udp",
				"_pdl-datastream._tcp",
				"_poch._tcp",
				"_pop3._tcp",
				"_postgresql._tcp",
				"_presence._tcp",
				"_printer._tcp",
				"_ptp._tcp",
				"_quinn._tcp",
				"_raop._tcp",
				"_rce._tcp",
				"_realplayfavs._tcp",
				"_rendezvouspong._tcp",
				"_riousbprint._tcp",
				"_rfb._tcp",
				"_rtsp._tcp",
				"_safarimenu._tcp",
				"_sallingclicker._tcp",
				"_scone._tcp",
				"_sdsharing._tcp",
				"_see._tcp",
				"_seeCard._tcp",
				"_serendipd._tcp",
				"_servermgr._tcp",
				"_shell._tcp",
				"_shout._tcp",
				"_shoutcast._tcp",
				"_soap._tcp",
				"_spike._tcp",
				"_spincrisis._tcp",
				"_spl-itunes._tcp",
				"_spr-itunes._tcp",
				"_ssh._tcp",
				"_ssscreenshare._tcp",
				"_strateges._tcp",
				"_sge-exec._tcp",
				"_sge-qmaster._tcp",
				"_stickynotes._tcp",
				"_sxqdea._tcp",
				"_sybase-tds._tcp",
				"_teamlist._tcp",
				"_teleport._udp",
				"_telnet._tcp",
				"_tftp._udp",
				"_ticonnectmgr._tcp",
				"_tinavigator._tcp",
				"_tryst._tcp",
				"_upnp._tcp",
				"_utest._tcp",
				"_vue4rendercow._tcp",
				"_webdav._tcp",
				"_whamb._tcp",
				"_wired._tcp",
				"_workstation._tcp",
				"_wormhole._tcp",
				"_workgroup._tcp",
				"_ws._tcp",
				"_xserveraid._tcp",
				"_xsync._tcp",
				"_xtshapro._tcp"
			});

			this.typeBox.Location = new System.Drawing.Point(8, 16);
			this.typeBox.Name = "typeBox";
			this.typeBox.Size = new System.Drawing.Size(192, 21);
			this.typeBox.Sorted = true;
			this.typeBox.TabIndex = 3;
			this.typeBox.SelectedIndexChanged += new System.EventHandler(this.typeBox_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 208);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "Name:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 240);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Type:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 272);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "Domain:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 304);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "Host:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nameField
			// 
			this.nameField.Location = new System.Drawing.Point(56, 208);
			this.nameField.Name = "nameField";
			this.nameField.ReadOnly = true;
			this.nameField.Size = new System.Drawing.Size(168, 20);
			this.nameField.TabIndex = 8;
			this.nameField.Text = "";
			// 
			// typeField
			// 
			this.typeField.Location = new System.Drawing.Point(56, 240);
			this.typeField.Name = "typeField";
			this.typeField.ReadOnly = true;
			this.typeField.Size = new System.Drawing.Size(168, 20);
			this.typeField.TabIndex = 9;
			this.typeField.Text = "";
			// 
			// domainField
			// 
			this.domainField.Location = new System.Drawing.Point(56, 272);
			this.domainField.Name = "domainField";
			this.domainField.ReadOnly = true;
			this.domainField.Size = new System.Drawing.Size(168, 20);
			this.domainField.TabIndex = 10;
			this.domainField.Text = "";
			// 
			// hostField
			// 
			this.hostField.Location = new System.Drawing.Point(56, 304);
			this.hostField.Name = "hostField";
			this.hostField.ReadOnly = true;
			this.hostField.Size = new System.Drawing.Size(168, 20);
			this.hostField.TabIndex = 11;
			this.hostField.Text = "";

			// 
			// portField
			// 
			this.portField.Location = new System.Drawing.Point(56, 336);
			this.portField.Name = "portField";
			this.portField.ReadOnly = true;
			this.portField.Size = new System.Drawing.Size(168, 20);
			this.portField.TabIndex = 12;
			this.portField.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 336);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 16);
			this.label5.TabIndex = 14;
			this.label5.Text = "Port:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// serviceTextField
			// 
			this.serviceTextField.HorizontalScrollbar = true;
			this.serviceTextField.Location = new System.Drawing.Point(264, 208);
			this.serviceTextField.Name = "serviceTextField";
			this.serviceTextField.Size = new System.Drawing.Size(232, 147);
			this.serviceTextField.TabIndex = 16;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 365);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.serviceTextField,
																		  this.label5,
																		  this.portField,
																		  this.hostField,
																		  this.domainField,
																		  this.typeField,
																		  this.nameField,
																		  this.label4,
																		  this.label3,
																		  this.label2,
																		  this.label1,
																		  this.typeBox,
																		  this.browseList});
			this.Name = "Form1";
			this.Text = "DNSServices Browser";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
		//
		// BrowseData
		//
		// This class is used to store data associated
		// with a DNSService.Browse() operation
		//
		public class BrowseData
		{
			public int		InterfaceIndex;
			public String	Name;
			public String	Type;
			public String	Domain;

			public override String
			ToString()
			{
				return Name;
			}

			public override bool
			Equals(object other)
			{
				bool result = false;

				if (other != null)
				{
					if ((object) this == other)
					{
						result = true;
					}
					else if (other is BrowseData)
					{
						BrowseData otherBrowseData = (BrowseData) other;

						result = (this.Name == otherBrowseData.Name);
					}
				}

				return result;
			}

			public override int
			GetHashCode()
			{
				return Name.GetHashCode();
			}
		};


		//
		// ResolveData
		//
		// This class is used to store data associated
		// with a DNSService.Resolve() operation
		//
		public class ResolveData
		{
			public int		InterfaceIndex;
			public String	FullName;
			public String	HostName;
			public int		Port;
			public Byte[]	TxtRecord;

			public override String
				ToString()
			{
				return FullName;
			}
		};

		//
		// Populate()
		//
		// Populate this form with data associated with a 
		// DNSService.Resolve() call
		//
		public void
		Populate(BrowseData browseData, ResolveData resolveData)
		{
			nameField.Text	= browseData.Name;
			typeField.Text	= browseData.Type;
			domainField.Text	= browseData.Domain;
			hostField.Text	= resolveData.HostName;
			portField.Text	= resolveData.Port.ToString();

			serviceTextField.Items.Clear();
		
			if (resolveData.TxtRecord != null)
			{
				for (int idx = 0; idx < TextRecord.GetCount(resolveData.TxtRecord); idx++)
				{
					String	key;
					Byte[]	bytes;
				
					bytes	= TextRecord.GetItemAtIndex(resolveData.TxtRecord, idx, out key);

					if (key.Length > 0)
					{
						String	val = "";

						if (bytes != null)
						{
							val = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
						}

						serviceTextField.Items.Add(key + "=" + val);
					}
				}
			}
		}

		//
		// called when the type field changes
		//
		private void typeBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			browseList.Items.Clear();

			if (browser != null)
			{
				browser.Dispose();
			}

			nameField.Text = "";
			typeField.Text = "";
			domainField.Text = "";
			hostField.Text = "";
			portField.Text = "";
			serviceTextField.Items.Clear();

			try
			{
				browser = DNSService.Browse(0, 0, typeBox.SelectedItem.ToString(), null, new DNSService.BrowseReply(OnBrowseReply));
			}
			catch
			{
				MessageBox.Show("Browse Failed", "Error");
				Application.Exit();
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (resolver != null)
			{
				resolver.Dispose();
			}

            if (browseList.SelectedItem != null)
            {
                try
                {
                    BrowseData data = (BrowseData) browseList.SelectedItem;

                    resolver = DNSService.Resolve(0, 0, data.Name, data.Type, data.Domain, new DNSService.ResolveReply(OnResolveReply));
                }
                catch
                {
                    MessageBox.Show("Resolve Failed", "Error");
                    Application.Exit();
                }
            }
		}

		//
		// OnAddService
		//
		// This method is "Invoked" by OnBrowseReply.  This call
		// executes in the context of the main thread
		//
		private void OnAddService
								(
								BrowseData	data
								)
		{
			browseList.Items.Add(data);
			browseList.Invalidate();
		}

		//
		// OnRemoveService
		//
		// This method is "Invoked" by OnBrowseReply.  This call
		// executes in the context of the main thread
		//
		private void OnRemoveService
								(
								BrowseData	data
								)
		{
			browseList.Items.Remove(data);
			browseList.Invalidate();
		}

		//
		// OnResolveService
		//
		// This method is "Invoked" by OnResolveReply.  This call
		// executes in the context of the main thread
		//
		private void OnResolveService
								(
								ResolveData	data
								)
		{
			resolver.Dispose();
			resolver = null;

			Populate((BrowseData) browseList.SelectedItem, data);
		}

		//
		// OnBrowseReply
		//
		// This call is invoked by the DNSService core.  It is
		// executed in the context of a worker thread, not the
		// main (GUI) thread.  We create a BrowseData object
		// and invoked the appropriate method in the GUI thread
		// so we can update the UI
		//
		private void OnBrowseReply
								(
								ServiceRef		sdRef,
								ServiceFlags	flags,
								int				interfaceIndex,
								ErrorCode		errorCode,
								String			name,
								String			type,
								String			domain
								)
		{
			if (errorCode == ErrorCode.NoError)
			{
				BrowseData data = new BrowseData();

				data.InterfaceIndex = interfaceIndex;
				data.Name = name;
				data.Type = type;
				data.Domain = domain;
	
				if ((flags & ServiceFlags.Add) != 0)
				{
					Invoke(addServiceCallback, new Object[]{data});
					
				}
				else if ((flags == 0) || ((flags & ServiceFlags.MoreComing) != 0))
				{
					Invoke(removeServiceCallback, new Object[]{data});
				}
			}
			else
			{
				MessageBox.Show("OnBrowseReply returned an error code: " + errorCode, "Error");
			}
		}

		private void OnResolveReply
								(
								ServiceRef		sdRef,  
								ServiceFlags	flags,
								int				interfaceIndex,
								ErrorCode		errorCode,
								String			fullName,
								String			hostName,
								int				port,
								Byte[]			txtRecord
								)
		{
			if (errorCode == ErrorCode.NoError)
			{
				ResolveData data = new ResolveData();

				data.InterfaceIndex = interfaceIndex;
				data.FullName		= fullName;
				data.HostName		= hostName;
				data.Port			= port;
				data.TxtRecord		= txtRecord;
	
				Invoke(resolveServiceCallback, new Object[]{data});
			}
			else
			{
				MessageBox.Show("OnResolveReply returned an error code: " + errorCode, "Error");
			}
		}
	}
}
