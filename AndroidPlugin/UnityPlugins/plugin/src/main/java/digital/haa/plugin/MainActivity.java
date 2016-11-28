package digital.haa.plugin;

import android.content.Intent;
import android.os.Bundle;

import com.unity3d.player.UnityPlayerActivity;

public class MainActivity extends UnityPlayerActivity
{
    public static String sessionIdIntent = "sessionId";
    private static String sessionId;

    public static String getMessage()
    {
        return "Plugin Reachable";
    }

    @Override
    protected void onCreate(Bundle var1)
    {
        Intent intent = getIntent();
        if(intent != null && intent.hasExtra(sessionIdIntent)) {
            sessionId = getIntent().getStringExtra(sessionIdIntent);
        }
        super.onCreate(var1);
    }

    public static String GetSessionId()
    {
        return sessionId;
    }
}
