using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CaesarCycleScriptPL : MonoBehaviour {

    public KMAudio Audio;
    public List<KMSelectable> keys;
    public GameObject[] dials;
    public TextMesh[] dialText;
    public TextMesh disp;

    //TRANSLATED BY GADDEMYTX

    private int r;
    private string[] message = new string[100] { "AFISZOWY", "AFRODYTA", "AKADEMIA", "AKTUALNY", "ALARMUJE", "ALKOHOLE", "BABECZKA", "BAKTERIA", "BARANINA", "BEZSILNY", "BEKONOWY", "BIELIZNA", "CELULOZA", "CERAMIKA", "CYRYLICA", "CYTRYNKA", "CZCIONKA", "CZWARTEK", "DEKAGRAM", "DESECZKA", "DETEKTOR", "DETEKTYW", "DRABINKA", "DRUKARKA", "EGOISTKA", "EGZOTYKA", "EKOLOGIA", "EKONOMIA", "ETYKIETA", "EWOLUCJA", "FALSTART", "FALOWANY", "FIKCYJNY", "FISZBINY", "FORMATOR", "FOTOMAPY", "GEODEZJA", "GEOLOGIA", "GLINIARZ", "GRAFFITI", "HARCERKA", "HAWAJSKA", "HOROSKOP", "HOMOGRAM", "IGRASZKA", "IGRZYSKA", "IMITACJA", "IMPREZKA", "JAPONIEC", "JASZCZUR", "JEDWABNE", "JOGURCIK", "KANTOREK", "KAPTUREK", "KIEROWCA", "KISZONKA", "LAMPIONY", "LIMUZYNY", "LOTNISKO", "LOGARYTM", "MAGISTER", "MARTWICA", "MASKOTKA", "MIEDNICA", "NAGROBEK", "NALATANE", "NEUTRONY", "NORMALNE", "OBEZNANY", "OBIECANY", "OJCZYZNA", "OSZUSTKA", "PACYFIZM", "PALEOLIT", "PARKOMAT", "PROFANUM", "REALISTA", "RECENZJA", "ROBACTWO", "ROCZNICA", "SELEKCJA", "SERWETKA", "SOCZEWKA", "SOSJERKA", "TANCERKA", "TECHNIKA", "TEKSTURA", "TEOLOGIA", "UFOLUDEK", "USTALONO", "UTOPIJNY", "UTOPIZMY", "WARIATKA", "WIBRACJE", "WIELORYB", "WILGOTNY", "ZACZYNAJ", "ZAKOPANE", "ZARZUTKA", "ZGRZEWKA"};
    private string[] response = new string[100] { "DRUKARKA", "GEODEZJA", "ETYKIETA", "KISZONKA", "BAKTERIA", "NEUTRONY", "ROCZNICA", "WILGOTNY", "CYTRYNKA", "FALSTART", "FORMATOR", "JASZCZUR", "DRABINKA", "PACYFIZM", "TEOLOGIA", "EKONOMIA", "HOMOGRAM", "JEDWABNE", "SOSJERKA", "ALKOHOLE", "KANTOREK", "PALEOLIT", "WIBRACJE", "NORMALNE", "MARTWICA", "GEOLOGIA", "HARCERKA", "NAGROBEK", "MASKOTKA", "IGRZYSKA", "AKTUALNY", "ALARMUJE", "LOGARYTM", "IMPREZKA", "GLINIARZ", "TEKSTURA", "SERWETKA", "BEKONOWY", "FIKCYJNY", "ZGRZEWKA", "ZARZUTKA", "BARANINA", "JOGURCIK", "OBEZNANY", "FALOWANY", "KIEROWCA", "MIEDNICA", "ZAKOPANE", "WARIATKA", "LIMUZYNY", "EKOLOGIA", "OJCZYZNA", "BEZSILNY", "BIELIZNA", "WIELORYB", "BABECZKA", "LOTNISKO", "FOTOMAPY", "DESECZKA", "AKADEMIA", "PARKOMAT", "REALISTA", "IGRASZKA", "MAGISTER", "IMITACJA", "UTOPIJNY", "EWOLUCJA", "TECHNIKA", "ROBACTWO", "JAPONIEC", "CZWARTEK", "DETEKTOR", "OSZUSTKA", "UFOLUDEK", "CZCIONKA", "KAPTUREK", "NALATANE", "CYRYLICA", "CELULOZA", "DEKAGRAM", "LAMPIONY", "HOROSKOP", "ZACZYNAJ", "SELEKCJA", "RECENZJA", "USTALONO", "SOCZEWKA", "AFISZOWY", "CERAMIKA", "UTOPIZMY", "HAWAJSKA", "TANCERKA", "AFRODYTA", "EGOISTKA", "FISZBINY", "PROFANUM", "DETEKTYW", "OBIECANY", "EGZOTYKA", "GRAFFITI" };
    private string[] ciphertext = new string[2];
    private string answer;
    private int[][] rot = new int[2][] { new int[8], new int[8]};
    private int pressCount;
    private bool moduleSolved;

    //Logging
    static int moduleCounter = 1;
    int moduleID;

    private void Awake()
    {
        moduleID = moduleCounter++;
        foreach(KMSelectable key in keys)
        {
            int k = keys.IndexOf(key);
            key.OnInteract += delegate () { KeyPress(k); return false; };
        }
    }

    void Start () {
        Reset();
	}

    private void KeyPress(int k)
    {
        keys[k].AddInteractionPunch(0.125f);
        if(moduleSolved == false)
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            if(k == 26)
            {
                pressCount = 0;
                answer = string.Empty;
            }
            else
            {
                pressCount++;
                answer = answer + "QWERTYUIOPASDFGHJKLZXCVBNM"[k];
            }
            disp.text = answer;
            if(pressCount == 8)
            {
                if(answer == ciphertext[1])
                {
                    moduleSolved = true;
                    Audio.PlaySoundAtTransform("InputCorrect", transform);
                    disp.color = new Color32(0, 255, 0, 255);
                }
                else
                {
                    GetComponent<KMBombModule>().HandleStrike();
                    disp.color = new Color32(255, 0, 0, 255);
                    Debug.LogFormat("[Caesar Cycle PL #{0}]The submitted response was {1}: Resetting", moduleID, answer);
                }
                Reset();
            }
        }
    }

    private void Reset() {

        StopAllCoroutines();
        if (moduleSolved == false)
        {
            pressCount = 0;
            answer = string.Empty;
            r = Random.Range(0, 100);
            string[] roh = new string[8];
            List<string>[] ciph = new List<string>[] { new List<string> { }, new List<string> { } };
            for (int i = 0; i < 8; i++)
            {
                dialText[i].text = string.Empty;
                rot[1][i] = rot[0][i];
                rot[0][i] = Random.Range(0, 8);
                roh[i] = rot[0][i].ToString();
                ciph[0].Add("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[("ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(message[r][i]) + rot[0][i]) % 26].ToString());
                ciph[1].Add("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[("ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(response[r][i]) + rot[0][i]) % 26].ToString());
            }
            ciphertext[0] = string.Join(string.Empty, ciph[0].ToArray());
            ciphertext[1] = string.Join(string.Empty, ciph[1].ToArray());
            Debug.LogFormat("[Caesar Cycle PL #{0}]The encrypted message was {1}", moduleID, ciphertext[0]);
            Debug.LogFormat("[Caesar Cycle PL #{0}]The dial rotations were {1}", moduleID, string.Join(", ", roh));
            Debug.LogFormat("[Caesar Cycle PL #{0}]The deciphered message was {1}", moduleID, message[r]);
            Debug.LogFormat("[Caesar Cycle PL #{0}]The response word was {1}", moduleID, response[r]);
            Debug.LogFormat("[Caesar Cycle PL #{0}]The correct response was {1}", moduleID, ciphertext[1]);
        }
        StartCoroutine(DialSet());
    }

    private IEnumerator DialSet()
    {
        int[] spin = new int[8];
        bool[] set = new bool[8];
        for (int i = 0; i < 8; i++)
        {
            if (moduleSolved == false)
            {
                spin[i] = rot[0][i] - rot[1][i];
            }
            else
            {
                spin[i] = - rot[0][i];
            }
            if(spin[i] < 0)
            {
                spin[i] += 8;
            }
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (spin[j] == 0)
                {
                    if (set[j] == false)
                    {
                        set[j] = true;
                        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);
                        if (moduleSolved == false)
                        {
                            dialText[j].text = ciphertext[0][j].ToString();
                        }
                        else
                        {
                            switch (j)
                            {
                                case 0:
                                    dialText[j].text = "S";
                                    break;
                                case 1:
                                    dialText[j].text = "U";
                                    break;
                                case 2:
                                    dialText[j].text = "P";
                                    break;
                                case 3:
                                    dialText[j].text = "E";
                                    break;
                                case 4:
                                    dialText[j].text = "R";
                                    break;
                                case 6:
                                    dialText[j].text = "W";
                                    break;
                                default:
                                    dialText[j].text = "O";
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    dials[j].transform.localEulerAngles += new Vector3(0, 0, 45);
                    spin[j]--;
                }
            }
            if (i < 7)
            {
                yield return new WaitForSeconds(0.25f);
            }
        }
        if(moduleSolved == true)
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            GetComponent<KMBombModule>().HandlePass();
        }
        disp.text = string.Empty;
        disp.color = new Color32(255, 255, 255, 255);
        yield return null;
    }
#pragma warning disable 414
    private string TwitchHelpMessage = "!{0} QWERTYUI [Inputs letters] | !{0} cancel [Deletes inputs]";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {

        if (command.ToLowerInvariant() == "cancel")
        {
            KeyPress(26);
            yield return null;
        }
        else
        {
            command = command.ToUpperInvariant();
            var word = Regex.Match(command, @"^\s*([A-Z\-]+)\s*$");
            if (!word.Success)
            {
                yield break;
            }
            command = command.Replace(" ", string.Empty);
            foreach (char letter in command)
            {
                KeyPress("QWERTYUIOPASDFGHJKLZXCVBNM".IndexOf(letter));
                yield return new WaitForSeconds(0.125f);
            }
            yield return null;
        }
    }
}
