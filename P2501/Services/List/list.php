<?php
	include_once('config.php');
	
	function ListDBConnect()
	{
		global $config_listDB_hostname;
		global $config_listDB_username;
		global $config_listDB_password;
		global $config_listDB_database;

		$db = mysql_pconnect($config_listDB_hostname,$config_listDB_username,$config_listDB_password);
        if (!$db)
			die("ERROR: Unable to conenct to database.\n");
        else
			$result = mysql_select_db($config_listDB_database);
				
		return $db;
	}
	
	header('Content-Type: text/plain');
	
	if (!defined('CONFIGURATION'))
		die("ERROR: Unable to load configuration.\n");
		
	$db = ListDBConnect();

	 if (array_key_exists('action',$_REQUEST))                         
		$action = $_REQUEST['action'];
	else
		$action = '';
		
	if ($action == 'viewlist')
		ViewList();
	else if ($action == 'addhost')
		AddHost();
	else if ($action == 'updatehost')
		UpdateHost();
	else if ($action == 'removehost')
		RemoveHost();

?>