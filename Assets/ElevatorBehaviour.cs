using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using System.Threading.Tasks;
public class ElevatorBehaviour : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Объявляем необходимые типы данных для последующей работы с ними
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    public int ElevatorPosition = 1;
    public string Floor1 = "Null", Floor2 = "Null", Floor3 = "Null", Floor4 = "Null", ElevatorCondition = "InActive", ElevatorDirection = "--";
    bool quit, stop = false;
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Здесь инициализируем кнопки, положения лифта, а также тестовые панели для вывода номера этажа и направления лифта
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    public Button Up1, Up2, Up3, Down2, Down3, Down4, FloorButton1, FloorButton2, FloorButton3, FloorButton4, Stop, Pause;
    public Image State1, State2, State3, State4;
    public Text ElevatorPanel, ElevatorDirect, Floor1Text, Floor2Text, Floor3Text, Floor4Text;


    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Функция Update обновляется каждый фрейм(frame), поэтому она отвечает за отображение изменения положения лифта и его направления на текстовых панелях
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        ElevatorPanel.text = ElevatorPosition.ToString();
        ElevatorDirect.text = ElevatorDirection.ToString();
        Floor1Text.text = ElevatorPosition.ToString();
        Floor2Text.text = ElevatorPosition.ToString();
        Floor3Text.text = ElevatorPosition.ToString();
        Floor4Text.text = ElevatorPosition.ToString();
    }
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    /*IEnumerator - базовый интерфейс для всех перечислителей, здесь выполняется задержка выполнения кода в целях эмуляции реального оборудования
     Сюда передается число, отображающее на сколько секунд необходимо выполнить задержку */
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator wait(float waitTime)
    {
        float counter = 0;

        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            if (quit)
            {
                yield break;
            }
            yield return null;
        }
    }

    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Данная функция отвечает за подъем лифта на определенный этаж, номер которого передается в переменную x
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator GoUp(int x)
    {
        int a = ElevatorPosition;
        if (a == x) //Åñëè ëèôò óæå íàõîäèòñÿ íà ýòàæå ñ âûçîâîì, òî ïðîèçîéäåò îòêðûòèå äâåðåé è ïîñàäêà ïàññàæèðîâ
        {
            if (a == 1) { Floor1 = "Null"; }
            else if (a == 2) { Floor2 = "Null"; }
            else if (a == 3) { Floor3 = "Null"; }
            else if (a == 4) { Floor4 = "Null"; }
            yield return wait(3);
        }
        for (int i = ElevatorPosition; i != x; i++) // Öèêë âûïîëíÿåòñÿ äî òåõ ïîð, ïîêà ëèôò íå ïðèåäåò íà íóæíûé ýòàæ
        {
            ElevatorDirection = "Up";
            switch (ElevatorPosition)
            {
                case 1:
                    yield return wait(1);
                    //gameObject.setActive îòâå÷àåò çà àêòèâàöèþ è äåàêòèâàöèþ êàðòèíêè ëèôòà â ïðèëîæåíèè
                    State1.gameObject.SetActive(false);
                    State2.gameObject.SetActive(true);
                    Floor1 = "Null";
                    ElevatorPosition = 2;
                    break;
                case 2:
                    yield return wait(1);
                    if (Floor2 == "Up") //Ïî ïóòè, åñëè íàïðàâëåíèå ëèôòà ñîâïàäàåò ñ êíîïêîé âûçîâà, òî ëèôò îñòàíîâèòñÿ
                    {
                        yield return wait(3);
                        Floor2 = "Null";//Ïîñëå îæèäàíèÿ ñòàòóñ âûçîâà êíîïêè íà äàííîì ýòàæå îáíóëÿåòñÿ 
                    }
                    State2.gameObject.SetActive(false);
                    State3.gameObject.SetActive(true);
                    ElevatorPosition = 3;
                    break;
                case 3:
                    yield return wait(1);
                    if (Floor3 == "Up")
                    {
                        yield return wait(3);
                        Floor3 = "Null";
                    }
                    State3.gameObject.SetActive(false);
                    State4.gameObject.SetActive(true);
                    ElevatorPosition = 4;
                    yield return wait(3);
                    break;
            }
            if (stop == true) //Äàííîå óñëîâèå ñðàáàòûâàåò, êîãäà íàæèìàåòñÿ êíîïêà STOP
            {
                stop = false;
                break;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        //Äàëåå âûïîëíÿþòñÿ ïðîâåðêè íà òî, íàæàòû ëè êíîïêè âûçîâà íà ýòàæàõ èëè íåò
        //Åñëè íàæàòû êíîïêè 1 èëè 4 ýòàæà, òî ñíà÷àëà ëèôò ïîåäåò íà íèõ, ïîñêîëüêó ïî ïóòè îí ìîæåò îñòàíîâèòüñÿ è ïîäîáðàòü ëþäåé íà äðóãèõ ýòàæàõ 
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        if (Floor1 == "Up" && ElevatorPosition >= 1)
        {
            StartCoroutine(GoDown(1));
        }
        else if (Floor4 == "Down" && ElevatorPosition <= 4)
        {
            StartCoroutine(GoUp(4));
        }
        else if (Floor3 != "Null" && ElevatorPosition <= 3)
        {
            StartCoroutine(GoUp(3));
        }
        else if (Floor3 != "Null" && ElevatorPosition >= 3)
        {
            StartCoroutine(GoDown(3));
        }
        else if (Floor2 != "Null" && ElevatorPosition <= 2)
        {
            StartCoroutine(GoUp(2));
        }
        else if (Floor2 != "Null" && ElevatorPosition >= 2)
        {
            StartCoroutine(GoDown(2));
        }
        //Åñëè âûçîâîâ áîëüøå íåò, òî ëèôò ñòàíîâèòñÿ íåàêòèâíûì
        else { ElevatorDirection = "--"; ElevatorCondition = "InActive"; }
    }
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Äàííàÿ ôóíêöèÿ îòâå÷àåò çà ñïóñê ëèôòà íà îïðåäåëåííûé ýòàæ è âî ìíîãîì ïîõîæà íà ôóíêöèþ ïîäúåìà
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator GoDown(int x)
    {
        int a = ElevatorPosition;
        if (a == x)
        {
            if (a == 1) { Floor1 = "Null"; }
            else if (a == 2) { Floor2 = "Null"; }
            else if (a == 3) { Floor3 = "Null"; }
            else if (a == 4) { Floor4 = "Null"; }
            yield return wait(3);//Æäåì ñåêóíäó äëÿ îòêðûòèÿ äâåðåé, ñåêóíäó äëÿ ïîñàäêè ïàññàæèðîâ è åù¸ ñåêóíäó äëÿ çàêðûòèÿ äâåðåé
        }
        for (int i = ElevatorPosition; i != x; i--)
        {
            ElevatorDirection = "Down";
            switch (ElevatorPosition)
            {
                case 2:
                    yield return wait(1); //Æäåì ñåêóíäó äëÿ ñèìóëÿöèè îæèäàíèÿ ïåðåõîäà ëèôòà ñ îäíîãî ýòàæà íà äðóãîé
                    if (Floor2 == "Down")
                    {
                        yield return wait(3);
                        Floor2 = "Null";
                    }
                    State2.gameObject.SetActive(false);
                    State1.gameObject.SetActive(true);
                    ElevatorPosition = 1;
                    yield return wait(3);
                    break;
                case 3:
                    yield return wait(1);
                    if (Floor3 == "Down")
                    {
                        yield return wait(3);
                        Floor3 = "Null";
                    }
                    State3.gameObject.SetActive(false);
                    State2.gameObject.SetActive(true);
                    ElevatorPosition = 2;
                    break;
                case 4:
                    yield return wait(1);
                    State4.gameObject.SetActive(false);
                    State3.gameObject.SetActive(true);
                    Floor4 = "Null";
                    ElevatorPosition = 3;
                    break;
            }
            if (stop == true)
            {
                stop = false;
                break;
            }
        }
        if (Floor1 == "Up" && ElevatorPosition > 1)
        {
            StartCoroutine(GoDown(1));
        }
        else if (Floor4 == "Down" && ElevatorPosition <= 4)
        {
            StartCoroutine(GoUp(4));
        }
        else if (Floor3 != "Null" && ElevatorPosition <= 3)
        {
            StartCoroutine(GoUp(3));
        }
        else if (Floor3 != "Null" && ElevatorPosition >= 3)
        {
            StartCoroutine(GoDown(3));
        }
        else if (Floor2 != "Null" && ElevatorPosition <= 2)
        {
            StartCoroutine(GoUp(2));
        }
        else if (Floor2 != "Null" && ElevatorPosition >= 2)
        {
            StartCoroutine(GoDown(2));
        }
        else { ElevatorDirection = "--"; ElevatorCondition = "InActive"; } 
    }

    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Äàëåå èäóò ôóíêöèè, ïðèâÿçàííûå ê îïðåäåëåííûì êíîïêàì è ðàáîòàþùèå â çàâèñèìîñòè îò òîãî àêòèâåí ëè ëèôò èëè íåò
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    public void FourthFloorButton()
    {
        if (ElevatorCondition == "InActive") //Åñëè ëèôò íåàêòèâåí, òî âûçûâàåì åãî íà ýòàæ
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(4));
        }
        else if (ElevatorCondition == "Active") //Åñëè ëèôò àêòèâåí, òî âûñòàâëÿåì ñòàòóñ êíîïêè â àêòèâíûé
        {
            Floor4 = "Down";
        }
    }
    public void ThirdFloorButtonUp()
    {
        if (ElevatorCondition == "InActive" && ElevatorPosition > 3)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(3));
        }
        else if (ElevatorCondition == "InActive" && ElevatorPosition < 3)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(3));
        }
        else if (ElevatorCondition == "Active")
        {
            Floor3 = "Up";
        }
    }
    public void ThirdFloorButtonDown()
    {
        if (ElevatorCondition == "InActive" && ElevatorPosition > 3)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(3));
        }
        else if (ElevatorCondition == "InActive" && ElevatorPosition < 3)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(3));
        }
        else if (ElevatorCondition == "Active")
        {
            Floor3 = "Down";
        }
    }
    public void SecondFloorButtonUp()
    {
        if (ElevatorCondition == "InActive" && ElevatorPosition > 2)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(2));
        }
        else if (ElevatorCondition == "InActive" && ElevatorPosition < 2)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(2));
        }
        else if (ElevatorCondition == "Active")
        {
            Floor2 = "Up";
        }
    }
    public void SecondFloorButtonDown()
    {
        if (ElevatorCondition == "InActive" && ElevatorPosition > 2)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(2));
        }
        else if (ElevatorCondition == "InActive" && ElevatorPosition < 2)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(2));
        }
        else if(ElevatorCondition == "Active")
        {
            Floor2 = "Down";
        }

    }
    public void FirstFloorButton()
    {
        if (ElevatorCondition == "InActive")
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(1));
        }
        else if (ElevatorCondition == "Active")
        {
            Floor1 = "Up";
        }
    }
    public void Elevator4Floor()
    {
        if (ElevatorCondition == "InActive")
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(4));
        }
        else if (ElevatorCondition == "Active")
        {
            Floor4 = "Down";
        }
    }
    public void Elevator3Floor()
    {
        if (ElevatorCondition == "InActive" && ElevatorPosition > 3)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(3));
        }
        else if (ElevatorCondition == "InActive" && ElevatorPosition < 3)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(3));
        }
        else if (ElevatorCondition == "Active" && ElevatorPosition <= 3)
        {
            Floor3 = "Up";
        }
        else if (ElevatorCondition == "Active" && ElevatorPosition >= 3)
        {
            Floor3 = "Down";
        }

    }
    public void Elevator2Floor()
    {
        if (ElevatorCondition == "InActive" && ElevatorPosition > 2)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(2));
        }
        else if (ElevatorCondition == "InActive" && ElevatorPosition < 2)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(2));
        }
        else if (ElevatorCondition == "Active" && ElevatorPosition <= 2)
        {
            Floor2 = "Up";
        }
        else if (ElevatorCondition == "Active" && ElevatorPosition >= 2)
        {
            Floor2 = "Down";
        }
    }
    public void Elevator1Floor()
    {
        if (ElevatorCondition == "InActive")
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(1));
        }
        else if (ElevatorCondition == "Active")
        {
            Floor1 = "Up";
        }
    }
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Êíîïêà STOP îñòàíàâëèâàåò ëèôò íà áëèæàéøåì ýòàæå (ñîãëàñíî äâèæåíèþ) è îòìåíÿåò âñå âûçîâû ëèôòà
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    public void STOP()
    {
        stop = true;
       Floor1 = "Null"; Floor2 = "Null"; Floor3 = "Null"; Floor4 = "Null"; ElevatorCondition = "InActive";
    }
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Êíîïêà PAUSE èìèòèðóåò âîñïðåïÿòñòîâàíèå çàêðûòèþ äâåðåé ëèôòà, à òàê æå ýêñòðåííóþ îñòàíîâêó ëèôòà íà ýòàæå
    //Ïîñëå ÷åãî ïðîäîëæàåò âûïîëíåíèå âûçîâîâ
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    public void PAUSE()
    {
        Thread.Sleep(2000);
    }
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Äàííàÿ êíîïêà â UNITY ïîçâîëÿåò âûéòè èç ïðèëîæåíèÿ
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    public void Exit()
    {
        Application.Quit();
    }

}
