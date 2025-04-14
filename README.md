# バブルの塔（苦行系協力アクション）

複数人がバブルボールに入って、**チームで協力しながら高所を目指す**、苦行系のオンライン協力アクションゲームです。  
足場やギミックに苦しみながら、仲間と呼吸を合わせて進むスリリングな体験を楽しめます。

## ゲーム概要
- プレイヤーたちはバブルボールに入った状態で物理演算に従って動く
- 互いにぶつかり合いながらも、高みを目指して全員で登る
- オンラインで協力プレイ可能（最大プレイヤー数：現在2人）

## 担当箇所（矢萩 / Allow-hub）

- **プレイヤー制御関連**  
  https://github.com/Allow-hub/SteamFocus/tree/master/Assets/SteamGame/Game/Scripts/InGame/Player
- **各エリアのギミック処理**  
  - 建築エリア（BuildingArea）  
    https://github.com/Allow-hub/SteamFocus/tree/master/Assets/SteamGame/Game/Scripts/InGame/BuildingArea  
  - 工場エリア（FactoryArea）  
    https://github.com/Allow-hub/SteamFocus/tree/master/Assets/SteamGame/Game/Scripts/InGame/FactoryArea  
  - 火山エリア（VolcanoArea）  
    https://github.com/Allow-hub/SteamFocus/tree/master/Assets/SteamGame/Game/Scripts/InGame/VolcanoArea  
- **シーン管理**  
  https://github.com/Allow-hub/SteamFocus/tree/master/Assets/SteamGame/Game/Scripts/ManagerScene  
- **オンライン同期（Photon2使用）**  
  https://github.com/Allow-hub/SteamFocus/tree/master/Assets/SteamGame/Game/Scripts/NetWork

## 工夫・挑戦したこと
- **Photon2を使用して、オンライン協力プレイを実装**しました。
- オブジェクトの同期処理やプレイヤー間の物理的な相互作用の設計にも挑戦しました。
- 各エリアごとにギミックを用意し、ステージごとの雰囲気や遊びの幅を意識しました。

## 反省点・改善したい点
- **物理挙動に関する処理が分散してしまい、バグの特定や調整が難しかった**です。
- 今後は、**物理演算を扱う責任を担うクラスやレイヤーを明確に分離**し、トラブル対応やデバッグをしやすくする設計を意識したいと感じました。

