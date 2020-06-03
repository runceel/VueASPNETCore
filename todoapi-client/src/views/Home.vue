<template>
  <div class="home">
    <LoginStatus />
    <img alt="Vue logo" src="../assets/logo.png">
    <HelloWorld msg="Welcome to Your Vue.js App"/>
  </div>
</template>

<script>
// @ is an alias to /src
import HelloWorld from '@/components/HelloWorld.vue'
import LoginStatus from '@/components/api-authorization/LoginState.vue';
export default {
  name: 'Home',
  components: {
    HelloWorld,
    LoginStatus,
  }
}
</script>
