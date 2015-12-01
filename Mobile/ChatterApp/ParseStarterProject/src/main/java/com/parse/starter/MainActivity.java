/*
 * Copyright (c) 2015-present, Parse, LLC.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 */
package com.parse.starter;

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.support.v7.app.ActionBarActivity;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.inputmethod.InputMethod;
import android.view.inputmethod.InputMethodManager;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;

import com.parse.FindCallback;
import com.parse.GetCallback;
import com.parse.ParseAnalytics;
import com.parse.Parse;
import com.parse.ParseACL;
import com.parse.ParseException;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.sql.Date;
import java.sql.Time;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;


public class MainActivity extends AppCompatActivity {

    final List<String> lsUsers = new ArrayList<String>();
    String chatWith = "";
    final String chatter = "Bobin";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        ParseAnalytics.trackAppOpenedInBackground(getIntent());
//
        ParseUser currentUser = ParseUser.getCurrentUser();
        currentUser.logOut();

        ParseQuery<ParseObject> query = ParseQuery.getQuery("message");
        query.whereNotEqualTo("sender", chatter);
//      query.selectKeys(Arrays.asList("sender"));

        query.findInBackground(new FindCallback<ParseObject>() {
            public void done(List<ParseObject> message, ParseException e) {
                if (e == null) {

                    for (int i = 0; i < message.size(); i++) {
                        if (!lsUsers.contains(message.get(i).get("sender").toString()))
                            lsUsers.add(message.get(i).get("sender").toString());
//                      arraySpinner[arraySpinner.length+1]=message.get(i).get("sender").toString();
                    }
                } else {
                }
                LoadSpinnerValues();

            }
        });
        final EditText edittext = (EditText) findViewById(R.id.chatInput);
        edittext.setOnKeyListener(new View.OnKeyListener() {
            public boolean onKey(View v, int keyCode, KeyEvent event) {
                // If the event is a key-down event on the "enter" button
                if ((event.getAction() == KeyEvent.ACTION_DOWN) &&
                        (keyCode == KeyEvent.KEYCODE_ENTER)) {
                    // Perform action on key press

                    InputMethodManager in = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);
                    ParseObject chatMessage = new ParseObject("message");
                    chatMessage.put("message", edittext.getText().toString());
                    chatMessage.put("receiver", chatWith);
                    chatMessage.put("sender", chatter);

                    ParseACL acl=new ParseACL();
                    acl.setPublicReadAccess(true);
                    chatMessage.setACL(acl);

                    chatMessage.saveInBackground();
                    edittext.setText("");

                    in.hideSoftInputFromWindow(edittext.getApplicationWindowToken(), InputMethodManager.HIDE_NOT_ALWAYS);

                    LoadChat();
                    return true;
                }
                return false;
            }
        });

    }

    public void LoadSpinnerValues() {
        Spinner s = (Spinner) findViewById(R.id.chatWith);
        ArrayAdapter<String> adapter = new ArrayAdapter<String>(this,
                android.R.layout.simple_spinner_item, lsUsers);
        s.setAdapter(adapter);

        s.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                chatWith = parent.getItemAtPosition(position).toString();
                LoadChat();
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

                // sometimes you need nothing here
            }
        });

        chatWith = s.getItemAtPosition(0).toString();
        LoadChat();
    }

    public void LoadChat() {

        ParseQuery<ParseObject> query = ParseQuery.getQuery("message");
//        String chatWithPerson = s.getSelectedItem().toString();
        String[] names = {chatWith, chatter};
        query.whereContainedIn("sender", Arrays.asList(names));
        query.whereContainedIn("receiver", Arrays.asList(names));
        query.addAscendingOrder("createdAt");

        query.findInBackground(new FindCallback<ParseObject>() {
            public void done(List<ParseObject> message, ParseException e) {
                if (e == null) {
                    TextView displayWindow = (TextView) findViewById(R.id.chatWindow);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < message.size(); i++) {
                        sb.append(message.get(i).get("sender").toString() + ": " + message.get(i).get("message").toString());
                        sb.append("\n");
                    }
                    displayWindow.setText(sb.toString());
                } else {
                }
            }
        });

        TimerTask abc = new TimerTask() {
            @Override
            public void run() {
                LoadChat();
            }
        };

        Timer def = new Timer();
        def.schedule(abc, 10000, 10000);
////        new Timer().scheduleAtFixedRate(LoadChat(chatWithName), 10, 2);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }
}
