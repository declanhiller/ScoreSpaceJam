using System;
using System.Collections;
using System.Collections.Generic;
using ChessMovements;
using Player;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PieceChooser : MonoBehaviour {

    [SerializeField] private Sprite rook;
    [SerializeField] private Sprite king;
    [SerializeField] private Sprite bishop;
    [SerializeField] private Sprite queen;
    [SerializeField] private Sprite knight;

    [SerializeField] private Image[] images;
    private ChessMovement[] selectableMovements;

    public Action onSelect;

    public List<Ability> possibleAbilities;

    public Ability displayedAbility;
    
    private void Awake() {
        possibleAbilities = new List<Ability>();
    }


    public void BringUpAbilities(Action onSelect, List<ChessMovement> chessMovements) {
        
        //Generate random ability
        int range = Random.Range(0, possibleAbilities.Count);
        displayedAbility = possibleAbilities[range];


        selectableMovements = new ChessMovement[images.Length];
        this.onSelect = onSelect;
        for (int i = 0; i < 3; i++) {
            ChessMovement movement = chessMovements[i];
            selectableMovements[i] = movement;
            if (movement.GetType() == typeof(RookMovement)) {
                images[i].sprite = rook;
            }else if (movement.GetType() == typeof(KingMovement)) {
                images[i].sprite = king;
            }else if (movement.GetType() == typeof(BishopMovement)) {
                images[i].sprite = bishop;
            }else if (movement.GetType() == typeof(QueenMovement)) {
                images[i].sprite = queen;
            }else if(movement.GetType() == typeof(KnightMovement)) {
                images[i].sprite = knight;
            }
        }
    }

    public void SelectPiece1() {
        selectableMovements[0].abilities.Add(displayedAbility);
        onSelect.Invoke();
    }

    public void SelectPiece2() {
        selectableMovements[0].abilities.Add(displayedAbility);
        onSelect.Invoke();
    }

    public void SelectPiece3() {
        selectableMovements[0].abilities.Add(displayedAbility);
        onSelect.Invoke();
    }
}
