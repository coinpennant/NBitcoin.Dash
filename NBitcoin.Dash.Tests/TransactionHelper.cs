using NBitcoin.RPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBitcoin.Dash.Tests
{
    public static class TransactionHelper
    {
        private static Network _network;

        static TransactionHelper()
        {
            NBitcoin.Dash.Networks.Register();
            _network = NBitcoin.Dash.Networks.Testnet;
        }

        /// <summary>
        /// Interactive example that walks through creating, signing, and broadcasting a Dash transaction. 
        /// Make sure you have Dash Core wallet running on TestNet for this example to work.
        /// </summary>
        public static void RunExample()
        {
            // Create a new HD extended private key
            ExtKey extKey = CreateExtKey();

            // Derive a child address so we can send it some coins
            var extPubKey = extKey.Neuter().GetWif(_network);
            int customerId = 1234567;
            string keyDerivationPath = $"{customerId}/0";
            BitcoinPubKeyAddress childAddress = DeriveChildAddress(extPubKey.ToString(), keyDerivationPath);

            // Watch this address in your core wallet so that you can monitor it for funds
            RPCClient rpc = GetRpcClient();

            rpc.ImportAddress(childAddress, $"Customer {customerId}", false);

            // Send some coins manually to the address just created
            Console.WriteLine($"Now, go to another wallet and send some coins to address {childAddress}.");
            Console.WriteLine($"Wait a few minutes for it to confirm, then press ENTER to continue.");
            Console.ReadLine();

            // Create and sign a transaction that spends the coins received by the address above.
            Console.WriteLine($"Enter an address of yours to which you want to receive coins:");
            string toAddr = Console.ReadLine();

            Console.WriteLine($"Enter the amount to spend:");
            string spend = Console.ReadLine();

            decimal minerFee = 0.00001m;
            decimal amountToSpend = Convert.ToDecimal(spend) - minerFee;

            Key privateKey = extKey.Derive(KeyPath.Parse(keyDerivationPath)).PrivateKey;

            var transaction = CreateAndSignTransaction(childAddress.ToString(), toAddr, amountToSpend, privateKey);

            // Finally, broadcast transaction to network 
            rpc.SendRawTransaction(transaction);
        }

        private static RPCClient GetRpcClient()
        {
            // Run your Dash Core locally like this: dash-qt.exe -server -testnet -rpcuser=user -rpcpassword=pass -rpcport=19998
            Uri uri = new Uri("http://127.0.0.1:19998");
            string rpcUser = "meuser";
            string rpcPass = "mepw99923";

            RPCCredentialString credentials = new RPCCredentialString();
            credentials.UserPassword = new System.Net.NetworkCredential(rpcUser, rpcPass);
            RPCClient client = new RPCClient(credentials, uri, _network);
            return client;
        }

        /// <summary>
        /// Create an HD private key
        /// </summary>
        private static ExtKey CreateExtKey()
        {
            Mnemonic mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
            ExtKey extKey = mnemonic.DeriveExtKey();
            return extKey;
        }

        /// <summary>
        /// Derive a child address from extended public key
        /// </summary>
        /// <param name="extPubKeyWif"></param>
        private static BitcoinPubKeyAddress DeriveChildAddress(string extPubKeyWif, string keyDerivationPath)
        {
            ExtPubKey extPubKey = ExtPubKey.Parse(extPubKeyWif, _network);
            BitcoinPubKeyAddress childAddress = extPubKey.Derive(KeyPath.Parse(keyDerivationPath)).PubKey.GetAddress(_network);
            return childAddress;
        }

        /// <summary>
        /// Create and sign a spending transaction.
        /// </summary>
        /// <param name="addrToSpend"></param>
        /// <param name="toAddr"></param>
        /// <param name="amount"></param>
        /// <param name="privateKey"></param>
        /// <returns>Signed Transaction object</returns>
        private static Transaction CreateAndSignTransaction(string addrToSpend, string toAddr, decimal amount, Key privateKey)
        {
            RPCClient client = GetRpcClient();

            // Create new transaction
            var transaction = new Transaction();

            // Get UTXOs in core wallet
            var unspentCoins = client.ListUnspent();

            // Build a new list of Coin for spending
            var coins = new List<Coin>();

            foreach (UnspentCoin unspentCoin in unspentCoins)
            {
                var addr = unspentCoin.Address;
                if (addr.ToString() == addrToSpend)
                {
                    // Create inputs
                    transaction.Inputs.Add(new TxIn()
                    {
                        PrevOut = unspentCoin.OutPoint,
                        ScriptSig = unspentCoin.Address.ScriptPubKey
                    });

                    // Create coin to spend
                    var coin = new Coin
                    (
                       unspentCoin.OutPoint.Hash,
                       unspentCoin.OutPoint.N,
                       unspentCoin.Amount,
                       unspentCoin.Address.ScriptPubKey
                    );

                    coins.Add(coin);
                }
            }

            // Create output/ destination
            var destAddress = BitcoinAddress.Create(toAddr, _network);

            // Use the passed-in "amount" value (make sure you subtracted miner fee before calling this function)
            TxOut output = new TxOut
            {
                Value = new Money(amount, MoneyUnit.BTC),
                ScriptPubKey = destAddress.ScriptPubKey
            };

            transaction.Outputs.Add(output);

            // Sign transaction
            transaction.Sign(privateKey, coins.ToArray());
            return transaction;
        }

    }
}
