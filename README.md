# Nightingale
Unity uGUI Expansion Package

#主な機能
https://twitter.com/leiyasuzumori/status/695450087070130176

既存のuGUIのスクリプトと組み合わせることで動くような仕組みでなるべく作ってます。

##TabGroup
グルーピングしたボタンを一つだけ選択できるようになります。

##AccordionButton
ボタンを押すと隠されていた表示領域が展開します。

このボタンを使ったScrollRectとの組み合わせなども検討中。

##CollectionView
アイテムのインベントリなどに使用されるn×mが実装できるスクロール領域。

現時点ではuGUIのScrollRectと組み合わせている為、有限数にのみ対応。

有限数の使い回しによる大量スクロールも実装予定。

##Utility
####ObjectPool
指定されたGameObjectを複数作成し使い回す為のクラス
