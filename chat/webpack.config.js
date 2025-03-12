const path = require("path");

module.exports = {
  mode: "production",
  entry: "./components/Chat.tsx",
  output: {
    path: path.resolve(__dirname, "dist"),
    filename: "chat-widget.js",
    library: "ChatWidget",
    libraryTarget: "umd",
    globalObject: "this",
  },
  module: {
    rules: [
      {
        test: /\.(js|jsx|ts|tsx)$/,
        exclude: /node_modules/,
        use: {
          loader: "babel-loader",
          options: {
            presets: ["@babel/preset-env", "@babel/preset-react", "@babel/preset-typescript"],
          },
        },
      },
    ],
  },
  externals: {
    react: "React",
    "react-dom": "ReactDOM",
    "@azure/communication-react": "AzureCommunicationUI",
  },
  resolve: {
    extensions: [".js", ".jsx", ".ts", ".tsx"],
  },
};
