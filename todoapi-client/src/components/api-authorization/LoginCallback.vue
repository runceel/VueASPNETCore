<template>
  <div>ログイン処理中…</div>
</template>

<script lang="ts">
import Vue from 'vue';
import store from '@/store';
export default Vue.extend({
  async created() {
    console.log('route: ' + JSON.stringify(this.$route.query));
    await store.auth.completeSignIn(this.$route.fullPath);
    const returnUrl = decodeURI(this.$route.query?.returnUrl as string ?? '/');
    this.$router.push(returnUrl);
  },
});
</script>
<style>

</style>