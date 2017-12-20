# NBitcoin.Dash

This project allows NBitcoin to support Dash.
To register Dash extensions, run:

```
NBitcoin.Dash.Networks.Register();
```

You can then use NBitcoin with `NBitcoin.Dash.Networks.Mainnet` or `NBitcoin.Dash.Networks.Testnet`.
Alternatively you can use `NBitcoin.Network.GetNetwork("dash-main")` to get the Network object.
You can then start using Dash in the same way you do with Bitcoin.

